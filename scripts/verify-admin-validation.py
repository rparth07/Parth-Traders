"""Create/update/delete ONLY named synthetic fixtures on the isolated local validation API."""
import json,urllib.request,urllib.error,urllib.parse,uuid
from pathlib import Path
import hashlib,urllib.parse
base='http://127.0.0.1:5080';private=json.loads(Path('.netlify/migration-secrets.json').read_text(encoding='utf-8'))
assert urllib.parse.urlsplit(private['DATABASE_URL']).hostname != urllib.parse.urlsplit(private['MIGRATION_DATABASE_URL']).hostname, 'Validation and production hosts must differ'
def call(path,method='GET',body=None,token=None,expected=200):
 data=None if body is None else json.dumps(body).encode();headers={'Content-Type':'application/json'}
 if token:headers['Authorization']='Bearer '+token
 try:
  with urllib.request.urlopen(urllib.request.Request(base+path,data=data,headers=headers,method=method),timeout=90) as r:status=r.status;raw=r.read()
 except urllib.error.HTTPError as e:status=e.code;raw=e.read()
 assert status==expected,path+' expected '+str(expected)+' got '+str(status)
 return json.loads(raw) if raw else None
health=call('/health/ready');assert health['databaseFingerprint']==hashlib.sha256(urllib.parse.urlsplit(private['MIGRATION_DATABASE_URL']).hostname.replace('-pooler','').encode()).hexdigest(), 'Wrong database target'
admin=call('/api/authentication/login','POST',{'username':private['BootstrapAdmin__userName'],'password':private['BootstrapAdmin__password']})['token']
ident=uuid.uuid4().hex[:8];category='Validation category '+ident;supplier='Validation supplier '+ident;product='Validation product & '+ident
created=[];before=call('/API/admin/products')
try:
 c=call('/API/admin/categories/add-category','POST',{'categoryName':category,'categoryDescription':'Isolated test fixture'},admin,201);created.append(('categories',category))
 s=call('/API/admin/suppliers/add-supplier','POST',{'supplierName':supplier,'supplierAddress':'Isolated test fixture','supplierEmail':'validation-'+ident+'@example.invalid','supplierPhoneNumber':'8'+str(int(ident,16))[-9:].zfill(9)},admin,201);created.append(('suppliers',supplier))
 p=call('/API/admin/products/add-product','POST',{'productName':product,'productSku':'VALIDATION-'+ident,'productDescription':'Isolated test fixture','productType':2,'productRating':4.2,'supplierName':supplier,'categoryName':category,'piecesPerUnit':1,'singlePieceMRP':25.99,'unitPrice':20.99,'discount':1.01,'unitsInStock':5},admin,201);created.append(('products',product));print('PASS isolated category/supplier/product create')
 p['unitPrice']=21.19;call('/API/admin/products/'+urllib.parse.quote(product,safe=''),'POST',p,admin)
 updated=call('/API/admin/products/'+urllib.parse.quote(product,safe=''));assert updated['unitPrice']==21.19 and updated['productType']==2 and updated['productRating']==4.2;print('PASS decimal product update and type/rating preservation')
 c['categoryDescription']='Updated isolated fixture';call('/API/admin/categories/'+urllib.parse.quote(category,safe=''),'POST',c,admin)
 s['supplierAddress']='Updated isolated fixture';call('/API/admin/suppliers/'+urllib.parse.quote(supplier,safe=''),'POST',s,admin);print('PASS category and supplier updates')
finally:
 for kind,name in reversed(created):call('/API/admin/'+kind+'/'+urllib.parse.quote(name,safe=''),'DELETE',token=admin,expected=204)
 assert len(call('/API/admin/products'))==len(before);print('PASS synthetic fixture cleanup; recovered products preserved')
