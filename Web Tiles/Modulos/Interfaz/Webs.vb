Imports System.Net
Imports Windows.Storage

Namespace Interfaz

    Module Webs

        Public anchoColumna As Integer = 250

        Public Sub Cargar()

            Dim recursos As New Resources.ResourceLoader

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim tbWebsEnlace As TextBox = pagina.FindName("tbWebsEnlace")

            AddHandler tbWebsEnlace.TextChanged, AddressOf WebsTextoCambia

            Dim botonWebsGenerar As Button = pagina.FindName("botonWebsGenerar")
            botonWebsGenerar.IsEnabled = False

            AddHandler botonWebsGenerar.Click, AddressOf WebsGenerarClick
            AddHandler botonWebsGenerar.PointerEntered, AddressOf EfectosHover.Entra_Boton_IconoTexto
            AddHandler botonWebsGenerar.PointerExited, AddressOf EfectosHover.Sale_Boton_IconoTexto

        End Sub

        Private Sub WebsTextoCambia(sender As Object, e As TextChangedEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonWebsGenerar As Button = pagina.FindName("botonWebsGenerar")

            Dim tb As TextBox = sender

            If tb.Text.Trim.Length > 2 Then
                botonWebsGenerar.IsEnabled = True
            Else
                botonWebsGenerar.IsEnabled = False
            End If

        End Sub

        Private Async Sub WebsGenerarClick(sender As Object, e As RoutedEventArgs)

            Dim boton As Button = sender
            boton.IsEnabled = False
            boton.Tag = e

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim pbWebsGenerar As ProgressBar = pagina.FindName("pbWebsGenerar")
            pbWebsGenerar.Visibility = Visibility.Visible

            Dim tbWebsEnlace As TextBox = pagina.FindName("tbWebsEnlace")
            Dim enlace As String = tbWebsEnlace.Text.Trim

            Dim wvWebGenerar As WebView = pagina.FindName("wvWebGenerar")
            Await WebView.ClearTemporaryWebDataAsync()

            If Not enlace.Contains("https://") And Not enlace.Contains("http://") Then
                enlace = "https://" + enlace
            End If

            wvWebGenerar.Navigate(New Uri(enlace))

            AddHandler wvWebGenerar.LoadCompleted, AddressOf WvGenerar

        End Sub

        Private Async Sub WvGenerar(sender As Object, e As NavigationEventArgs)

            Dim frame As Frame = Window.Current.Content
            Dim pagina As Page = frame.Content

            Dim botonWebsGenerar As Button = pagina.FindName("botonWebsGenerar")

            Dim wv As WebView = sender
            Dim enlace As String = wv.Source.AbsoluteUri
            Dim html As String = Await wv.InvokeScriptAsync("eval", New String() {"document.documentElement.outerHTML;"})

            If Not html = Nothing Then
                Try
                    Dim mutear As String = "var videos = document.querySelectorAll('video'),
                                            audios = document.querySelectorAll('audio');
                                            [].forEach.call(videos, function(video) { video.muted = true; });
                                            [].forEach.call(audios, function(audio) { audio.muted = true; });"
                    Await wv.InvokeScriptAsync("eval", New String() {mutear})
                Catch ex As Exception

                End Try

                Dim titulo As String = Web_Tiles.Webs.SacarTitulo(html)

                If Not titulo = Nothing Then
                    titulo = WebUtility.HtmlDecode(titulo)
                End If

                Dim dominio As String = enlace

                If enlace.IndexOf("/", 10) > -1 Then
                    Dim int As Integer = enlace.IndexOf("/", 10)
                    dominio = dominio.Remove(int, dominio.Length - int)
                End If

                Dim icono As String = Web_Tiles.Webs.SacarIcono(html)

                If Not icono = Nothing Then
                    If Not icono.Contains("https://") And Not icono.Contains("http://") Then
                        If icono.Contains("//") Then
                            If icono.IndexOf("//") = 0 Then
                                icono = "https:" + icono
                            Else
                                icono = dominio + icono
                            End If
                        Else
                            icono = dominio + icono
                        End If
                    End If
                End If

                Dim id As String = enlace
                id = id.Replace(".", Nothing)
                id = id.Replace(":", Nothing)
                id = id.Replace("/", Nothing)
                id = id.Replace("\", Nothing)
                id = id.Replace("-", Nothing)
                id = id.Replace(">", Nothing)
                id = id.Replace("<", Nothing)
                id = id.Replace("?", Nothing)
                id = id.Replace("|", Nothing)

                wv.Width = 930
                wv.Height = 450
                Await Task.Delay(1000)
                Dim ficheroAncha As StorageFile = Await ApplicationData.Current.LocalFolder.CreateFileAsync(id + "_ancha.png", CreationCollisionOption.ReplaceExisting)
                Dim imagenAncha As String = ficheroAncha.Path
                Await ImagenFichero.Generar(ficheroAncha, wv, wv.ActualWidth, wv.ActualHeight)

                wv.Width = 620
                wv.Height = 620
                Await Task.Delay(1000)
                Dim ficheroGrande As StorageFile = Await ApplicationData.Current.LocalFolder.CreateFileAsync(id + "_grande.png", CreationCollisionOption.ReplaceExisting)
                Dim imagenGrande As String = ficheroGrande.Path
                Await ImagenFichero.Generar(ficheroGrande, wv, wv.ActualWidth, wv.ActualHeight)

                Dim web As New Tile(titulo, id, enlace, icono, icono, imagenAncha, imagenGrande)
                botonWebsGenerar.Tag = web

                Web_Tiles.Webs.BotonTile_Click()
            End If

            botonWebsGenerar.IsEnabled = True

            Dim pbWebsGenerar As ProgressBar = pagina.FindName("pbWebsGenerar")
            pbWebsGenerar.Visibility = Visibility.Collapsed

            Dim tbWebsEnlace As TextBox = pagina.FindName("tbWebsEnlace")
            tbWebsEnlace.Text = String.Empty

        End Sub

    End Module

End Namespace