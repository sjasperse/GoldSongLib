rm -rf ./cert
mkdir -p cert

# PASSWORD=`openssl rand -hex 32`
# echo $PASSWORD > cert/password.txt
# openssl genpkey -out cert/private_key.pem -algorithm rsa -pass file:cert/password.txt
# openssl rsa -pubout -passin file:cert/password.txt -in cert/private_key.pem -out cert/public_key.pem

openssl genpkey -out cert/private_key.pem -algorithm rsa

openssl rsa -pubout -in cert/private_key.pem -out cert/public_key.pem



