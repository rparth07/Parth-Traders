"""Isolated validation checks. Default local URL must use the migration-validation database."""
import json,urllib.request,urllib.error,uuid,secrets
from decimal import Decimal
from pathlib import Path
import hashlib,urllib.parse
base='http://127.0.0.1:5080'
private=json.loads(Path('.netlify/migration-secrets.json').read_text(encoding='utf-8'))
assert urllib.parse.urlsplit(private['DATABASE_URL']).hostname != urllib.parse.urlsplit(private['MIGRATION_DATABASE_URL']).hostname, 'Validation and production hosts must differ'
def call(path,method='GET',body=None,token=None,expected=200):
 data=None if body is None else json.dumps(body).encode();headers={'Content-Type':'application/json'}
 if token:headers['Authorization']='Bearer '+token
 try:
  with urllib.request.urlopen(urllib.request.Request(base+path,data=data,headers=headers,method=method),timeout=90) as r:status=r.status;raw=r.read()
 except urllib.error.HTTPError as e:status=e.code;raw=e.read()
 assert status==expected, path+' expected '+str(expected)+' got '+str(status)
 return json.loads(raw) if raw else None
health=call('/health/ready');assert health['status']=='ready';expected=hashlib.sha256(urllib.parse.urlsplit(private['MIGRATION_DATABASE_URL']).hostname.replace('-pooler','').encode()).hexdigest();assert health['databaseFingerprint']==expected, 'API is not using the validation database';print('PASS verified validation database readiness')
products=call('/API/admin/products');source=json.loads(Path('Backend/Parth Traders/Recovery/products-source.json').read_text(encoding='utf-8'))
assert len(products)==50
by_sku={p['productSku']:p for p in products}
for row in source:
 p=by_sku[row['ProductSku']]
 for field in ['UnitPrice','SinglePieceMRP','Discount','UnitsInStock','ProductRating']:
  key=field[0].lower()+field[1:];assert str(p[key])==str(row[field]) or float(p[key])==float(row[field]),field
print('PASS 50 products match approved SQL prices, discounts, stock and ratings')
manifest=json.loads(Path('Backend/Parth Traders/Recovery/product-categories.proposed.json').read_text())
assert all(by_sku[r['productSku']]['categoryName']==r['categoryName'] for r in manifest)
assert len(call('/API/admin/categories'))==9
print('PASS all 50 owner-approved category assignments and nine categories')
login=call('/api/authentication/login','POST',{'username':private['BootstrapAdmin__userName'],'password':private['BootstrapAdmin__password']});admin=login['token']
assert not any(k in login['admin'] for k in ['passwordHash','securityStamp','concurrencyStamp']);print('PASS sanitized admin login')
call('/API/admin/suppliers',expected=401);call('/API/admin/suppliers',token=admin);print('PASS anonymous admin access denied and admin read allowed')
ident=uuid.uuid4().hex[:10];email='validation-'+ident+'@example.invalid';password=secrets.token_urlsafe(18)+'aA1!';name='validation-'+ident
call('/api/user/authentication/signup','POST',{'userName':name,'phoneNumber':'9'+str(int(ident,16))[-9:].zfill(9),'customerAddress':'Isolated validation fixture','email':email,'password':password},expected=201)
customer=call('/api/user/authentication/login','POST',{'email':email,'password':password});token=customer['token']
assert not any(k in customer['customer'] for k in ['passwordHash','securityStamp','concurrencyStamp']);print('PASS customer signup and sanitized login')
profile=call('/api/user/authentication/customer-details/'+customer['customer']['id'],'POST',{'customerAddress':'Updated isolated fixture'},token);assert profile['customer']['customerAddress']=='Updated isolated fixture'
call('/api/user/authentication/customer-details/not-the-owner','POST',{'customerAddress':'test'},token,expected=403);print('PASS owned customer profile update without reauthentication')
call('/api/authentication/admin-details/'+login['admin']['id'],'POST',{'firstName':login['admin']['firstName']},admin);print('PASS administrator profile round-trip with existing token')
call('/API/admin/suppliers',token=token,expected=403);call('/API/orders/'+name,token=token);call('/API/orders/another-user',token=token,expected=403)
call('/api/authentication','POST',{},expected=401);print('PASS role and order ownership boundaries')
call('/API/orders','POST',{'transactionId':'forged-test','userName':name,'orderDetails':[],'grandTotal':1},token,expected=409);print('PASS client-trusted paid orders rejected')
conf=call('/api/payments/config');assert conf['mode']=='sandbox';assert 'clientSecret' not in conf;print('PASS sandbox public configuration omits secret')
if conf['enabled']:
 call('/api/payments/orders','POST',[],token,expected=400)
 order=call('/api/payments/orders','POST',[{'sku':products[0]['productSku'],'quantity':1}],token)
 assert Decimal(str(order['total']))==(Decimal(str(products[0]['unitPrice']))*(100-Decimal(str(products[0]['discount'])))/100).quantize(Decimal('0.01'), rounding=__import__('decimal').ROUND_HALF_UP);call('/api/payments/orders/'+order['id'],token=token)
 safety=next(p for p in products if p['productSku']=='SG-10007')
 order=call('/api/payments/orders','POST',[{'sku':safety['productSku'],'quantity':2}],token)
 assert Decimal(str(order['total']))==Decimal('25.58')
 print('PASS sandbox OAuth and percentage-priced orders including 20% safety glasses (no capture)')
Path('.netlify/validation-fixture.json').write_text(json.dumps({'email':email,'password':password,'userName':name}),encoding='utf-8')
print('PASS validation fixture saved privately; no production business records changed')
