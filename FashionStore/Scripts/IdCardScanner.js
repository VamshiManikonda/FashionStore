var IdCardScanner = {
    ReadyToScanImage : false,
    ArrayIdDocment :null,
    myControl: null,
    Initialise: function (AutoScan) {
        try {
        myControl = document.getElementById("ReaderActiveX");
       
            if (myControl.IsConnected == false) {
                myControl.LogLevel = 0;

                myControl.Configuration.EnableRF = true;
                myControl.Configuration.EnableDG1 = true;
                myControl.Configuration.ImageRFPhoto = true;

                myControl.Configuration.ImageIR = true;
                myControl.Configuration.ImageVIS = true;
                myControl.Configuration.ImageUV = true;
                myControl.Configuration.ImagePhoto = true;

                myControl.Configuration.SetImageFormat = "JPG";  // or "BMP"
                myControl.Configuration.ImageScale = 40;
                myControl.Configuration.JPGQuality = 75;

                myControl.Configuration._1D = false;
                myControl.Configuration.PDF417 = false;
                myControl.Configuration.DataMatrix = false;
                myControl.Configuration.Aztec = false;
                myControl.Configuration.QRCode = false;

                if (myControl.Connect(AutoScan) == false) {
                    // alert("Unable to Initialise SDK");
                }
                else {
                    //  alert("SDK Initialised");
                    //myControl.Test(100);
                }
            }
        }
        catch (ex) {
            alert("Oops An Error, error message is: " + ex.Description);
        }
    },
    Disconnect: function () {
        try{
            myControl = document.getElementById("ReaderActiveX");
            myControl.Disconnect();
        }catch(e)
        {

        }
    },
    FillOpticalData: function () {
       // $('#home #txtSearchkey').val(myControl.MRZOpticalData.DocumentNumber);
    },
    FillChipData: function () {
       // $('#home #txtSearchkey').val(myControl.MRZChipData.DocumentNumber);
    }
};


function ReaderActiveX::ReaderEvent(evnt) {
    //alert(evnt);
    switch (evnt) {
        case "DOC_ON_WINDOW":
            // alert("Got a Doc");
            break;
        case "DOC_REMOVED":
            //alert("Doc Gone");
            //ClearData();
            break;
        case "END_OF_DOCUMENT_DATA":
            break;
        default:
    }
}

function ReaderActiveX::ReaderData(evnt) {
    //document.getElementById('ReaderData').value = evnt;
    switch (evnt) {
        case "CD_CODELINE":
            IdCardScanner.FillOpticalData();
            break;
        case "CD_CODELINE_DATA":
            IdCardScanner.FillOpticalData();
            break;
        case "CD_SCDG1_CODELINE_DATA":
            IdCardScanner.FillChipData();
            break;
        case "CD_IMAGEVIS":
            if (IdCardScanner.ReadyToScanImage == true) {
                if (IdCardScanner.ArrayIdDocment.length < 2) {
                    IdCardScanner.ArrayIdDocment.push(myControl.ImageData.ImageVIS.Base64);
                    $("#divScanDocs").append("<img style='float:left;width:50%;height:300px;' src='data:image/png;base64," + myControl.ImageData.ImageVIS.Base64 + "' />");
                } else {
                    alert("Already you scanned both side. Please clear the documents and rescan again.");
                     
                }
            }
            break;
        case "CD_BARCODE_1D_128":
        case "CD_BARCODE_1D_3_OF_9":
        case "CD_BARCODE_1D_CODEABAR":
        case "CD_BARCODE_1D_CODE_93":
        case "CD_BARCODE_1D_IATA_2_OF_5":
        case "CD_BARCODE_1D_INDUSTRIAL_2_OF_5":
        case "CD_BARCODE_1D_INTERLEAVED_2_OF_5":
        case "CD_BARCODE_1D_UPD_EAN":
        case "CD_BARCODE_AZTECCODE":
        case "CD_BARCODE_DATAMATRIX":
        case "CD_BARCODE_PDF417":
        case "CD_BARCODE_QRCODE":
            //$('#home #txtSearchkey').val(myControl.BarCodeData);
            break
        default:
            //alert(evnt);
    }
}