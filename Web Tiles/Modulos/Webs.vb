Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports Windows.UI.Xaml.Media.Animation

Module Webs

    Public Function SacarTitulo(html As String)

        If html.Contains("<title>") Then
            Dim int As Integer = html.IndexOf("<title>")
            Dim temp As String = html.Remove(0, int + 7)

            Dim int2 As Integer = temp.IndexOf("</title>")
            Dim temp2 As String = temp.Remove(int2, temp.Length - int2)

            Return temp2.Trim
        End If

        Dim lista As New List(Of String) From {
            "og:title",
            "twitter:title",
            "og:site_name",
            "application-name",
            "apple-mobile-web-app-title"
        }

        For Each intento In lista
            Dim resultado As String = ExtraerDatoMeta(html, intento)

            If Not resultado = Nothing Then
                Return resultado
                Exit For
            End If
        Next

        Return Nothing

    End Function

    Public Function SacarIcono(html As String)

        Dim lista As New List(Of String) From {
            "og:image",
            "twitter:image",
            "image_src"
        }

        For Each intento In lista
            Dim resultado As String = ExtraerDatoMeta(html, intento)

            If Not resultado = Nothing Then
                Return resultado
                Exit For
            End If
        Next

        Dim lista2 As New List(Of String) From {
            "apple-touch-icon",
            "shortcut icon",
            "icon",
            "SHORTCUT ICON"
        }

        For Each intento In lista2
            Dim resultado As String = ExtraerDatoLink(html, intento)

            If Not resultado = Nothing Then
                Return resultado
                Exit For
            End If
        Next

        Return Nothing

    End Function

    Private Function ExtraerDatoMeta(html As String, etiqueta As String)

        Dim html2 As String = html

        If html2.Contains("<meta") Then
            Dim i As Integer = 0
            While i < 100
                If html2.Contains("<meta") Then
                    Dim int As Integer = html2.IndexOf("<meta")
                    Dim temp As String = html2.Remove(0, int + 5)

                    Dim temp2 As String = html2.Remove(0, int)
                    Dim int2 As Integer = temp2.IndexOf(">")
                    temp2 = temp2.Remove(int2, temp2.Length - int2)

                    html2 = temp

                    If temp2.Contains(ChrW(34) + etiqueta + ChrW(34)) Then
                        Dim int3 As Integer = temp2.IndexOf("content=")
                        Dim temp3 As String = temp2.Remove(0, int3 + 9)

                        Dim int4 As Integer = temp3.IndexOf(ChrW(34))
                        Dim temp4 As String = temp3.Remove(int4, temp3.Length - int4)

                        Return temp4.Trim
                        Exit While
                    End If
                Else
                    Exit While
                End If
                i += 1
            End While
        End If

        Return Nothing

    End Function

    Private Function ExtraerDatoLink(html As String, etiqueta As String)

        Dim html2 As String = html

        If html2.Contains("<link") Then
            Dim i As Integer = 0
            While i < 100
                If html2.Contains("<link") Then
                    Dim int As Integer = html2.IndexOf("<link")
                    Dim temp As String = html2.Remove(0, int + 5)

                    Dim temp2 As String = html2.Remove(0, int)
                    Dim int2 As Integer = temp2.IndexOf(">")
                    temp2 = temp2.Remove(int2, temp2.Length - int2)

                    html2 = temp

                    If temp2.Contains(ChrW(34) + etiqueta + ChrW(34)) Then
                        Dim int3 As Integer = temp2.IndexOf("href=")
                        Dim temp3 As String = temp2.Remove(0, int3 + 6)

                        Dim int4 As Integer = temp3.IndexOf(ChrW(34))
                        Dim temp4 As String = temp3.Remove(int4, temp3.Length - int4)

                        Return temp4.Trim
                        Exit While
                    End If
                Else
                    Exit While
                End If
                i += 1
            End While
        End If

        Return Nothing

    End Function

    Public Sub BotonTile_Click()

        Trial.Detectar()
        Interfaz.AñadirTile.ResetearValores()

        Dim frame As Frame = Window.Current.Content
        Dim pagina As Page = frame.Content

        Dim botonWebsGenerar As Button = pagina.FindName("botonWebsGenerar")
        Dim juego As Tile = botonWebsGenerar.Tag

        Dim botonAñadirTile As Button = pagina.FindName("botonAñadirTile")
        botonAñadirTile.Tag = juego

        Dim imagenJuegoSeleccionado As ImageEx = pagina.FindName("imagenJuegoSeleccionado")
        imagenJuegoSeleccionado.Source = juego.ImagenAncha

        Dim tbJuegoSeleccionado As TextBlock = pagina.FindName("tbJuegoSeleccionado")
        tbJuegoSeleccionado.Text = juego.Titulo

        Dim gridAñadirTile As Grid = pagina.FindName("gridAñadirTile")
        Interfaz.Pestañas.Visibilidad(gridAñadirTile, juego.Titulo, Nothing)

        '---------------------------------------------

        ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("animacionJuego", botonWebsGenerar)
        Dim animacion As ConnectedAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("animacionJuego")

        If Not animacion Is Nothing Then
            animacion.Configuration = New BasicConnectedAnimationConfiguration
            animacion.TryStart(gridAñadirTile)
        End If

        '---------------------------------------------

        Dim tbImagenTituloTextoTileAncha As TextBox = pagina.FindName("tbImagenTituloTextoTileAncha")
        tbImagenTituloTextoTileAncha.Text = juego.Titulo
        tbImagenTituloTextoTileAncha.Tag = juego.Titulo

        Dim tbImagenTituloTextoTileGrande As TextBox = pagina.FindName("tbImagenTituloTextoTileGrande")
        tbImagenTituloTextoTileGrande.Text = juego.Titulo
        tbImagenTituloTextoTileGrande.Tag = juego.Titulo

        '---------------------------------------------

        Dim imagenPequeña As ImageEx = pagina.FindName("imagenTilePequeña")
        imagenPequeña.Source = Nothing

        Dim imagenMediana As ImageEx = pagina.FindName("imagenTileMediana")
        imagenMediana.Source = Nothing

        Dim imagenAncha As ImageEx = pagina.FindName("imagenTileAncha")
        imagenAncha.Source = Nothing

        Dim imagenGrande As ImageEx = pagina.FindName("imagenTileGrande")
        imagenGrande.Source = Nothing

        If Not juego.ImagenPequeña = Nothing Then
            imagenPequeña.Source = juego.ImagenPequeña
            imagenPequeña.Tag = juego.ImagenPequeña
        End If

        If Not juego.ImagenMediana = Nothing Then
            imagenMediana.Source = juego.ImagenMediana
            imagenMediana.Tag = juego.ImagenMediana
        End If

        If Not juego.ImagenAncha = Nothing Then
            imagenAncha.Source = juego.ImagenAncha
            imagenAncha.Tag = juego.ImagenAncha
        End If

        If Not juego.ImagenGrande = Nothing Then
            imagenGrande.Source = juego.ImagenGrande
            imagenGrande.Tag = juego.ImagenGrande
        End If

    End Sub

End Module
