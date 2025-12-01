using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter; 
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PruebasUI
{
    [TestFixture]
    public class MisPruebas
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private static ExtentReports extent;
        private ExtentTest test;


        private const string UrlBase = "https://localhost:7083";

        //  CONFIGURACIÓN DEL REPORTE (Se ejecuta una vez al inicio)
        [OneTimeSetUp]
        public void SetupGlobal()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            // ExtentSparkReporter es para versiones 5.x de ExtentReports
            var reporter = new ExtentSparkReporter(Path.Combine(path, "ReporteFinal.html"));
            reporter.Config.DocumentTitle = "Tarea 4 - Selenium";
            reporter.Config.ReportName = "Resultados de Pruebas Automatizadas";

            extent = new ExtentReports();
            extent.AttachReporter(reporter);
        }



        //  ABRIR NAVEGADOR (Antes de cada test)
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }


        //  CERRAR NAVEGADOR Y LIMPIAR (Después de cada test)
        [TearDown]
        public void Teardown()
        {
            var estado = TestContext.CurrentContext.Result.Outcome.Status;
            var mensaje = TestContext.CurrentContext.Result.Message;

            try
            {
               
                if (driver != null)
                {
                    string foto = TomarCaptura(TestContext.CurrentContext.Test.Name);
                    test.AddScreenCaptureFromPath(foto);
                }
            }
            catch (Exception)
            {
               
                test.Info("Nota: No se pudo tomar captura de pantalla porque el navegador se cerró.");
            }
            finally
            {
               
                if (estado == NUnit.Framework.Interfaces.TestStatus.Failed)
                    test.Fail("FALLO: " + mensaje);
                else
                    test.Pass("EXITOSO");

                // 3. Cerramos el navegador
                if (driver != null)
                {
                    try { driver.Quit(); driver.Dispose(); } catch { }
                    driver = null;
                }
            }
        }

        //  GUARDAR REPORTE HTML 
        [OneTimeTearDown]
        public void Finalizar() => extent.Flush();


       
        //  LAS HISTORIAS DE USUARIO
       

        [Test] // HU-01: Login Exitoso
        public void T01_Login_Exitoso()
        {
            driver.Navigate().GoToUrl(UrlBase);
            driver.FindElement(By.Id("txtUsuario")).SendKeys("admin");
            driver.FindElement(By.Id("txtPassword")).SendKeys("123");
            driver.FindElement(By.Id("btnIngresar")).Click();

            Assert.IsTrue(wait.Until(d => d.Url.Contains("Libros")), "No entró al sistema");
        }

        [Test] // HU-02: Login Fallido
        public void T02_Login_Fallido()
        {
            driver.Navigate().GoToUrl(UrlBase);
            driver.FindElement(By.Id("txtUsuario")).SendKeys("admin");
            driver.FindElement(By.Id("txtPassword")).SendKeys("MAL");
            driver.FindElement(By.Id("btnIngresar")).Click();

            string error = wait.Until(d => d.FindElement(By.Id("msgError"))).Text;
            
            Assert.IsTrue(error.Contains("incorrectas"), "No mostró mensaje de error");
        }


        [Test] // HU-05: Cerrar Sesión
        public void T05_Cerrar_Sesion()
        {
            HacerLogin();

            driver.Navigate().GoToUrl(UrlBase + "/Acceso/Salir");

           
            System.Threading.Thread.Sleep(2000);

            
            bool redirigioCorrectamente = !driver.Url.Contains("/Salir") &&
                                           driver.PageSource.Contains("Iniciar Sesión");

            Assert.IsTrue(redirigioCorrectamente, "No cerró sesión correctamente");
        }


        [Test] // HU-03: Crear Libro
        public void T03_Crear_Libro()
        {
            HacerLogin();

            
            driver.FindElement(By.Id("btnNuevoLibro")).Click();

          
            string randomId = DateTime.Now.Ticks.ToString();
            string titulo = "Libro " + randomId.Substring(randomId.Length - 5);
            string isbnUnico = randomId.Substring(0, 10); // ISBN unico

            //  Llenar formulario
            driver.FindElement(By.Id("txtTitulo")).SendKeys(titulo);
            driver.FindElement(By.Id("txtAutor")).SendKeys("Bot Selenium");
            driver.FindElement(By.Id("txtIsbn")).SendKeys(isbnUnico); 

            //  Guardar
            driver.FindElement(By.Id("btnGuardar")).Click();

            // Validar redirección 
            bool cambioPagina = wait.Until(d => d.Url.Contains("Index") || d.Url.EndsWith("Libros"));
            Assert.IsTrue(cambioPagina, "No redirigió al Index. Posiblemente el ISBN estaba repetido o el modelo inválido.");

            //  Validar que el libro está en la tabla
            bool existe = wait.Until(d => d.PageSource.Contains(titulo));
            Assert.IsTrue(existe, "El libro se guardó pero no aparece en la tabla");
        }




        [Test] // HU-05: Límite Título 
        public void T05_Limite_Titulo()
        {
            HacerLogin();
            driver.FindElement(By.Id("btnNuevoLibro")).Click();

            // Intentar guardar 60 caracteres (el límite es 50)
            driver.FindElement(By.Id("txtTitulo")).SendKeys(new string('X', 60));
            driver.FindElement(By.Id("txtAutor")).SendKeys("Test");
            driver.FindElement(By.Id("btnGuardar")).Click();

            
            bool sigoEnCreate = driver.Url.Contains("Create");
            Assert.IsTrue(sigoEnCreate, "El sistema permitió guardar un título demasiado largo");
        }




        [Test] // HU-04: Eliminar Libro (Soft Delete)
        public void T04_Eliminar_Libro()
        {
            HacerLogin();

            // Verificar si hay libros para borrar
            var botones = driver.FindElements(By.CssSelector(".btn-eliminar"));
            if (botones.Count == 0) Assert.Ignore("No hay libros para eliminar. Ejecuta T03 primero.");

            botones.Last().Click(); 

           
            wait.Until(d => d.PageSource.Contains("Desactivar"));
            driver.FindElement(By.Id("btnConfirmarEliminar")).Click();

            wait.Until(d => d.Url.Contains("Index"));
        }



        [Test] // HU-06: Editar Libro (CRUD - Update)
        public void T06_Editar_Libro()
        {
            HacerLogin();

            
            var botonesEditar = driver.FindElements(By.CssSelector(".btn-editar"));

           
            if (botonesEditar.Count == 0) Assert.Ignore("No hay libros para editar. Ejecuta T03 primero.");

   
            botonesEditar.Last().Click();

            var txtAutor = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("txtAutorEditar")));
            txtAutor.Clear();
            txtAutor.SendKeys("Autor Editado PRO");

            //  Guardar
            driver.FindElement(By.Id("btnGuardarEdicion")).Click();

            bool cambioExitoso = wait.Until(d => d.PageSource.Contains("Autor Editado PRO"));
            Assert.IsTrue(cambioExitoso, "El sistema no guardó la edición correctamente.");
        }


        private void HacerLogin()
        {
            driver.Navigate().GoToUrl(UrlBase);
            driver.FindElement(By.Id("txtUsuario")).SendKeys("admin");
            driver.FindElement(By.Id("txtPassword")).SendKeys("123");
            driver.FindElement(By.Id("btnIngresar")).Click();

            wait.Until(d => d.Url.Contains("Libros"));
        }

        private string TomarCaptura(string nombre)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", nombre + ".png");
            screenshot.SaveAsFile(path);
            return path;
        }
    }
}
