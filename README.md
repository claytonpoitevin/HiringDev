# Desafio técnico para desenvolvedores

Construa uma nova solução restful, utilizando no backend e no front os frameworks de sua preferência, a qual deverá conectar na API do YouTube e disponibilizar as seguintes funcionalidades:

- Botão para pesquisar canal/video;
- Listar os canais/videos encontrados e salvos no banco;
- Visualizar os detalhes de cada canal/video.

Alguns requisitos:

- Deve ser uma aplicação totalmente nova;
- A solução deve estar em um repositório público do GitHub;
- A aplicação deve armazenar as informações encontradas;
- Utilizar MongoDB,  MySQL ou Postgres;
- O deploy deve ser realizado, preferencialmente na AWS;
- A aplicação precisa ter testes automatizados.

Quando terminar, faça um Pull Request neste repo e avise-nos por email.

**IMPORTANTE:** se você não conseguir finalizar o teste, por favor nos diga o motivo e descreva quais foram as suas dificuldades. Claro que você também pode sugerir uma outra abordagem para avaliarmos seus skills técnicos, mas é com você para vender seu peixe, mostrar-nos do que é capaz.


------------------------

Clayton Poitevin - 2020-03-29

Aspnet Core 3.1 - WebAPI 
Aspnet Core 3.1 - MVC
MongooDB

Considerações acerca do teste:

- Fiz o deploy para o AWS conforme requisitado, mas não é o ambiente que estou familiarizado. No meu dia-a-dia utilizo o Microsoft Azure, portanto posso ter feito algo errado. Também não fiz os projetos em containers por não estar familiarizado, mas conheço parcialmente o processo. O script que fiz para automatizar o deploy está em ./deploy_eb_aws.sh
Os links do projeto já com deploy são:
http://awseb-e-u-AWSEBLoa-9OGLZBL63GU4-421507736.us-east-1.elb.amazonaws.com/ - api
http://awseb-e-m-awsebloa-11n2c7i899ml2-1618593881.us-east-1.elb.amazonaws.com/ - client
mongodb://ec2-54-88-183-215.compute-1.amazonaws.com:27017 - mongodb
- Também fiz os testes automatizados para cumprir com o requisito apesar de não fazer parte do meu dia-a-dia. Sei do que se trata e como devem funcionar, mas a equipe de qualidade de onde trabalho atualmente não enxerga o ganho em "perder tempo" com testes de unidade, pois estão cobertos com os testes automatizados simulando o uso (SmartBear TestComplete).
- O meu forte não é front-end. Sempre que necessário costumo a fazer algumas coisas na web com o front, mas como trabalhamos com software desktop com serviços da nuvem, acabo não praticando tanto.
- Quanto ao que foi pedido, não ficou claro se gostariam que, além de armazenar as informações do retorno da pesquisa do YouTube, também deveriamos utilizar os resultados inseridos de alguma forma. Portanto, fiz uma página de "detalhes" do vídeo que busca os detalhes direto do banco.