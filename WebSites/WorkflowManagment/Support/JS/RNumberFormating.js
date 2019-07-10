function ValidateNumberKeyPress(field, evt)
        {
            var charCode = (evt.which) ? evt.which : event.keyCode
            var keychar = String.fromCharCode(charCode);

            if (charCode > 31 && (charCode < 48 || charCode > 57) && keychar != "."  && keychar != "-" )
            {
                return false;
            }

            if (keychar == "." && field.value.indexOf(".") != -1) 
            {
                return false;
            }
                
            if(keychar == "-")
            {
                if (field.value.indexOf("-") != -1 /* || field.value[0] == "-" */) 
                {
                    return false;
                }
                else
                {
                    //save caret position
                    var caretPos = getCaretPosition(field);
                    if(caretPos != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        function ValidateNumberKeyUp(field)
        {
            if(document.selection.type == "Text")
            {
                return;
            }
        
             //save caret position
            var caretPos = getCaretPosition(field);
            
            var fdlen = field.value.length;
       
            UnFormatNumber(field);
 
            var IsFound = /^-?\d+\.{0,1}\d*$/.test(field.value);
            if(!IsFound)
            {
                setSelectionRange(field, caretPos, caretPos);
                return false;             
            }
            
            field.value = FormatNumber(field.value);
            
            fdlen = field.value.length - fdlen;

            
            setSelectionRange(field, caretPos+fdlen, caretPos+fdlen);
        }

        function ValidateAndFormatNumber(NumberTextBox)
        {
            if(NumberTextBox.value == "") return;
            
            UnFormatNumber(NumberTextBox);

            var IsFound = /^-?\d+\.{0,1}\d*$/.test(NumberTextBox.value);
            if(!IsFound)
            {
                alert("Not a number");
                NumberTextBox.focus();
                NumberTextBox.select();  
                return;             
            }
            
            if(isNaN(parseFloat(NumberTextBox.value)))
            {
                alert("Number exceeding float range");
                NumberTextBox.focus();
                NumberTextBox.select();               
            }

            NumberTextBox.value = FormatNumber(NumberTextBox.value);
        }
        
        function FormatNumber(fnum)
        {
            var orgfnum = fnum;
            var flagneg = false;
            
            if(fnum.charAt(0) == "-")
            {
                flagneg = true;
                fnum = fnum.substr(1, fnum.length-1);
            }
            
            psplit = fnum.split(".");

            var cnum = psplit[0],
	            parr = [],
	            j = cnum.length,
	            m = Math.floor(j / 3),
	            n = cnum.length % 3 || 3;

            // break the number into chunks of 3 digits; first chunk may be less than 3
            for (var i = 0; i < j; i += n) {
	            if (i != 0) {n = 3;}
	            parr[parr.length] = cnum.substr(i, n);
	            m -= 1;
            }

            // put chunks back together, separated by comma
            fnum = parr.join(",");

            // add the precision back in
            //if (psplit[1]) {fnum += "." + psplit[1];}
            if (orgfnum.indexOf(".") != -1)  
            {
                fnum += "." + psplit[1];
            }
            
            if(flagneg == true)
            {
                fnum = "-" + fnum;
            }
            
            return fnum;
        }
           
        function UnFormatNumber(obj)
        {
            if(obj.value == "") return;
            
            obj.value = obj.value.replace(/,/gi, "");
        }
        
        function getCaretPosition(objTextBox){

            var objTextBox = window.event.srcElement;

            var i = objTextBox.value.length;

            if (objTextBox.createTextRange){
                objCaret = document.selection.createRange().duplicate();
                while (objCaret.parentElement()==objTextBox &&
                  objCaret.move("character",1)==1) --i;
            }
            return i;
        }

        function setSelectionRange(input, selectionStart, selectionEnd) {
            if (input.setSelectionRange) {
                input.focus();
                input.setSelectionRange(selectionStart, selectionEnd);
            }
            else if (input.createTextRange) {
                var range = input.createTextRange();
                range.collapse(true);
                range.moveEnd('character', selectionEnd);
                range.moveStart('character', selectionStart);
                range.select();
            }
        }
