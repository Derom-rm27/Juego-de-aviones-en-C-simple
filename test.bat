@echo off
:: Asegura que el script se ejecute en el directorio actual
cd /d "%~dp0"

echo ================================
echo      CONFIGURACION DE ENTORNO
echo ================================

:: 1. Verifica si Python esta instalado
python --version >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Python no esta instalado o no esta en el PATH.
    echo Por favor instalalo manualmente desde:
    echo https://www.python.org/downloads/
    echo.
    pause
    exit /b
) ELSE (
    echo [OK] Python detectado.
)

:: 2. Crea entorno virtual
echo.
echo --------------------------------------------
echo 2. Verificando entorno virtual...
IF NOT EXIST "entorno_avion" (
    echo Creando entorno virtual 'entorno_avion'...
    python -m venv entorno_avion
) ELSE (
    echo El entorno ya existe.
)

echo Activando entorno...
call entorno_avion\Scripts\activate

:: 3. Actualiza pip
echo.
echo --------------------------------------------
echo 3. Actualizando pip...
python -m pip install --upgrade pip

:: 4. Instala paquetes necesarios
echo.
echo --------------------------------------------
echo 4. Instalando librerias (pytest, pyautogui, cv2, pillow)...
pip install pytest pytest-html pyautogui pillow opencv-python

:: 5. Mensaje final
echo.
echo ============================================
echo           INSTALACION COMPLETA
echo ============================================
echo.
echo Entorno listo. Puedes ejecutar tus pruebas con:
echo python test_avion.py --html=reporte_juego.html
echo.
pause