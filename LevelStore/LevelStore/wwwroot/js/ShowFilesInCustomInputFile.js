
var inputs;
$(document).ready(function() {
    inputs = document.querySelectorAll('.inputfile');
    Array.prototype.forEach.call(inputs,
        function(input) {
            var label = input.nextElementSibling,
                labelVal = label.innerHTML;

            input.addEventListener('change',
                function(e) {
                    var fileName = '';
                    if (this.files && this.files.length > 1) {
                        fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}',
                            this.files.length);
                        fileName = fileName.replace("files selected", "файлов выбрано");
                    } else
                        fileName = e.target.value.split('\\').pop();

                    if (fileName)
                        label.querySelector('span').innerHTML = fileName;
                    else
                        label.innerHTML = labelVal;
                });
        });
});