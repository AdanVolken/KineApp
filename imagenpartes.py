import os
import sqlite3
from PIL import Image
import io

# Rutas a la carpeta de imágenes y la base de datos
carpeta_imagenes = r'C:\Users\adanv\OneDrive\Escritorio\Partes del cuerpo'
ruta_db = r'C:\Users\adanv\OneDrive\Escritorio\Programacion\Lenguajes\c#\KineApp\Resources\Raw\Kine.db'

# Conectar a la base de datos
conn = sqlite3.connect(ruta_db)
cursor = conn.cursor()

# Función para convertir una imagen a binario
def convertir_imagen_a_binario(ruta_imagen):
    with Image.open(ruta_imagen) as img:
        with io.BytesIO() as output:
            img.save(output, format=img.format)
            return output.getvalue()

# Recorrer todas las imágenes en la carpeta
for nombre_archivo in os.listdir(carpeta_imagenes):
    if nombre_archivo.endswith(('png', 'jpg', 'jpeg', 'bmp', 'gif')):
        nombre_parte, extension = os.path.splitext(nombre_archivo)
        ruta_imagen = os.path.join(carpeta_imagenes, nombre_archivo)
        
        # Convertir la imagen a binario
        imagen_binaria = convertir_imagen_a_binario(ruta_imagen)
        
        # Actualizar la tabla en la base de datos
        cursor.execute("""
            UPDATE Parte
            SET ImgParte = ?
            WHERE Nombre = ?
        """, (imagen_binaria, nombre_parte))
        
        print(f'Imagen para {nombre_parte} actualizada.')

# Guardar los cambios y cerrar la conexión
conn.commit()
conn.close()