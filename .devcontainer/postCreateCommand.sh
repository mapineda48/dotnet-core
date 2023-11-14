#!/bin/bash

apt-get update
apt-get install -y ca-certificates curl gnupg
mkdir -p /etc/apt/keyrings
curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg

echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_20.x nodistro main" | tee /etc/apt/sources.list.d/nodesource.list

apt-get update
apt-get install nodejs -y

# if [[ -v GIT_EMAIL ]] && [[ -v GIT_USER ]] && [[ -v GITHUB_USER ]] && [[ -v GITHUB_TOKEN ]]; then  
#     git config --global --add safe.directory /home/njs
    
#     # Configura tu nombre de usuario y correo electrónico de Git
#     git config --global user.name "$GIT_USER"
#     git config --global user.email "$GIT_EMAIL"


#     # Configura el origen remoto, reemplaza 'tu_repositorio' con el nombre de tu repositorio
#     git remote add origin https://$GITHUB_USER:$GITHUB_TOKEN@github.com/$GITHUB_USER/njs.git
# fi  