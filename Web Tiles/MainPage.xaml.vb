Public NotInheritable Class MainPage
    Inherits Page

    Private Sub Nv_Loaded(sender As Object, e As RoutedEventArgs)

        Dim recursos As New Resources.ResourceLoader()

        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Webs"), FontAwesome5.EFontAwesomeIcon.Solid_Globe))
        nvPrincipal.MenuItems.Add(Interfaz.NavigationViewItems.Generar(recursos.GetString("Config"), FontAwesome5.EFontAwesomeIcon.Solid_Cog))
        nvPrincipal.MenuItems.Add(New NavigationViewItemSeparator)

    End Sub

    Private Sub Nv_ItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)

        Dim recursos As New Resources.ResourceLoader

        Dim item As TextBlock = args.InvokedItem

        If gridProgreso.Visibility = Visibility.Collapsed Then
            If Not item Is Nothing Then
                If item.Text = recursos.GetString("Webs") Then
                    Interfaz.Pestañas.Visibilidad(gridWebs, item.Text, item)
                ElseIf item.Text = recursos.GetString("Config") Then
                    Interfaz.Pestañas.Visibilidad(gridConfig, item.Text, item)
                End If
            End If
        End If

    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)

        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "es-ES"
        'Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US"

        Cache.Cargar()
        Interfaz.Webs.Cargar()
        Interfaz.AñadirTile.Cargar()
        MasTiles.Cargar()
        MasCosas.Cargar()

        Dim recursos As New Resources.ResourceLoader
        Interfaz.Pestañas.Visibilidad(gridWebs, recursos.GetString("Webs"), Nothing)

        ElementSoundPlayer.State = ElementSoundPlayerState.Off

    End Sub

End Class
