﻿Imports Windows.Graphics.Imaging
Imports Windows.Storage
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Module ImagenFichero

    Public Async Function Generar(fichero As StorageFile, objeto As Object, ancho As Integer, alto As Integer) As Task

        Dim resultadoRender As New RenderTargetBitmap()
        Await resultadoRender.RenderAsync(objeto)
        Dim buffer As IBuffer = Await resultadoRender.GetPixelsAsync
        Dim pixeles As Byte() = buffer.ToArray
        Dim rawdpi As DisplayInformation = DisplayInformation.GetForCurrentView()

        Using stream As IRandomAccessStream = Await fichero.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.AllowReadersAndWriters)
            Dim encoder As BitmapEncoder = Await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream)
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, resultadoRender.PixelWidth, resultadoRender.PixelHeight, rawdpi.RawDpiX, rawdpi.RawDpiY, pixeles)

            encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant
            encoder.BitmapTransform.ScaledWidth = ancho
            encoder.BitmapTransform.ScaledHeight = alto

            Await encoder.FlushAsync
            stream.Dispose()
        End Using

    End Function

    Public Async Sub Exportar(boton As Button)

        Dim ficheroImagen As New List(Of String) From {
            ".jpg"
        }

        Dim guardarPicker As New FileSavePicker With {
            .SuggestedStartLocation = PickerLocationId.PicturesLibrary
        }

        guardarPicker.SuggestedFileName = "imagenbase"
        guardarPicker.FileTypeChoices.Add("Imagen", ficheroImagen)

        Dim ficheroResultado As StorageFile = Await guardarPicker.PickSaveFileAsync

        If Not ficheroResultado Is Nothing Then
            Await ImagenFichero.Generar(ficheroResultado, boton, boton.ActualWidth, boton.ActualHeight)
        End If

    End Sub

End Module
