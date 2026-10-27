"""Signup regression checks; hard-bound to the isolated local validation API."""
import concurrent.futures,hashlib,json,secrets,urllib.request,urllib.error,urllib.parse,uuid
from pathlib import Path
base='http://127.0.0.1:5080'
private=json.loads(Path('.netlify/migration-secrets.json').read_text(encoding='utf-8'))
assert urllib.parse.urlsplit(private['DATABASE_URL']).hostname!=urllib.parse.urlsplit(private['MIGRATION_DATABASE_URL']).hostname

def call(path,body=None):
    request=urllib.request.Request(base+path,data=None if body is None else json.dumps(body).encode(),headers={'Content-Type':'application/json'})
    try:
        with urllib.request.urlopen(request,timeout=90) as response:return response.status,json.loads(response.read() or b'null')
    except urllib.error.HTTPError as error:return error.code,json.loads(error.read() or b'null')

health=call('/health/ready')[1]
expected=hashlib.sha256(urllib.parse.urlsplit(private['MIGRATION_DATABASE_URL']).hostname.replace('-pooler','').encode()).hexdigest()
assert health['databaseFingerprint']==expected,'Refusing registration tests outside the validation database'
print('PASS validation database fingerprint')
ident=uuid.uuid4().hex[:12]
phone='9'+str(secrets.randbelow(10**9)).zfill(9)
password=secrets.token_hex(5)[:-1]+'7'
short_password=secrets.token_hex(3)+'7'
letter_password=''.join(secrets.choice('abcdefghijklmnopqrstuvwxyz') for _ in range(10))
body={'userName':'signup-'+ident,'phoneNumber':'+91 '+phone[:5]+' '+phone[5:],'customerAddress':'Isolated signup regression fixture','email':'signup-'+ident+'@example.invalid','password':password}
def signup(change=None):return call('/api/user/authentication/signup',{**body,**(change or {})})
def error_fields(response):return {key.lower() for key in response.get('errors',{})}

for label,change,field in [
    ('short password',{'password':short_password},'password'),
    ('missing password digit',{'password':letter_password},'password'),
    ('username with spaces',{'userName':'Signup Test '+ident},'username'),
    ('invalid phone',{'phoneNumber':'1234'},'phonenumber'),
    ('invalid email',{'email':'not-an-email'},'email'),
    ('missing address',{'customerAddress':' '},'customeraddress')]:
    status,data=signup(change);assert status==400,(label,status);assert field in error_fields(data),(label,error_fields(data));print('PASS field-specific '+label)
status,data=signup({'password':short_password,'userName':'Invalid Name','phoneNumber':'1234'})
assert status==400 and len(error_fields(data))>=3
print('PASS multiple field errors returned together')
status,data=signup();assert status==201,status
status,login=call('/api/user/authentication/login',{'email':body['email'],'password':password});assert status==200
assert login['customer']['phoneNumber']=='+91'+phone
assert not any(key in login['customer'] for key in ['passwordHash','securityStamp'])
print('PASS ten-character password and formatted international phone create a usable account')

for label,change,field in [
    ('email',{'userName':'email-'+ident,'email':body['email'].upper(),'phoneNumber':'8'+phone[1:]},'email'),
    ('username',{'email':'other-'+ident+'@example.invalid','phoneNumber':'7'+phone[1:]},'username'),
    ('formatted phone',{'userName':'phone-'+ident,'email':'phone-'+ident+'@example.invalid','phoneNumber':'+91('+phone[:5]+')-'+phone[5:]},'phone')]:
    status,data=signup(change);assert status==409,(label,status);assert field in error_fields(data),(label,error_fields(data));print('PASS duplicate '+label+' is an actionable conflict')

racephone='6'+str(secrets.randbelow(10**9)).zfill(9)
def race(n):
    return signup({'userName':'race-'+str(n)+'-'+ident,'email':'race-'+str(n)+'-'+ident+'@example.invalid','phoneNumber':racephone})
with concurrent.futures.ThreadPoolExecutor(max_workers=2) as pool:results=list(pool.map(race,[1,2]))
assert sorted(status for status,_ in results)==[201,409],[status for status,_ in results]
assert 'phone' in error_fields(next(data for status,data in results if status==409))
print('PASS concurrent duplicate phone returns conflict instead of service failure')
Path('.netlify/signup-fixture.json').write_text(json.dumps({'userName':body['userName'],'email':body['email'],'password':password,'phone':phone,'formattedPhone':body['phoneNumber']}),encoding='utf-8')
print('PASS synthetic signup fixture saved privately; no production account touched')
