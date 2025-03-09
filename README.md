# Chat App using TLS

This is a console chat messenger that uses TLS for encryption. The application follows a client-server architecture. The client sends messages to the server, which saves them to a PostgreSQL database and forwards them to another client. The recipient client can read the message and reply.
### The server uses encryption to store passwords, which are saved in the database in encrypted form. The server can handle multiple clients concurrently.
![image](https://github.com/user-attachments/assets/bcaabaa7-3dc7-46d9-9a45-b525313c5441)


For database operations, the application uses Microsoft.EntityFrameworkCore. Password encryption is implemented via System.Security.Cryptography.X509Certificates, while TLS communication is enabled using System.Net.Sockets.TcpClient, System.Net.Sockets.TcpListener, and System.Net.Security.SslStream.

### Here’s what the user chat page interface looks like
![image](https://github.com/user-attachments/assets/f34d0b36-b49d-42d6-8d02-067bcfefc540)
![image](https://github.com/user-attachments/assets/83718a75-b57d-4083-94c2-4f3bb8333da6)
![image](https://github.com/user-attachments/assets/a734a654-3b9a-4c86-bbd1-adfda7b44825)


