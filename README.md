# Bionic-University-ASP.NET-
Homeworks, I made during ASP.NET training in Bionic-University (Kiev)

1. Biis (Bicycle IIS)
Simple web server, that just listens localhost and specified port (modifiable in config) and send response to client as static html web pages and custom dynamic .biis pages. GET and POST actions handling is also available. Client requests are handled in multi-thread, so it's ok to send two requests in a time (if ya can, of course)
2. Chromefox
Simple web browser on WPF, which sends raw GET requests by url and fetches basic content from response
3. Fakebook
Little social network, built on WinForms. You can register, login, re-login with different name, make posts, like posts of ya and other users and different stuff. Features : forms authentication, self-made styling (it's crappy, I know, that's my first experience in HTML and CSS). Register information is not stored in Db, so you should re-register every time you launch app.
4. GuitarShop
At last, ASP.NET MVC app, which is, basically, a lightweight shop, where you can choose guitar, order it and you'll be freakin' happy. Tasty features : bootstrap, client and server validation, async controller, error handling.
