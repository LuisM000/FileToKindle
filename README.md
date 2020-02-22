# FileToKindle
Descarga, convierte y envía tus archivos al email de tu kindle partir de un magnet link (sólo disponible para macOS).

# Desde consola</br>
Configura el archivo email.json con los datos de tu email autorizado

{
  "Address": "tu_email_autorizado@email.com",
  "Password": "tu_contraseña",
  "Host": "smtp.office365.com",
  "Port": 587
}

Abre la aplicación de consola, configura el email de tu kindle y añade los magnet links
process -e 'tu_email_kindle@kindle.com' -m 'magnetLink1' 'magnetLink2'</br></br>
![Console App](images/consoleapp.gif)


# Desde app de escritorio</br>
![Desktop App](images/desktopapp.gif)
