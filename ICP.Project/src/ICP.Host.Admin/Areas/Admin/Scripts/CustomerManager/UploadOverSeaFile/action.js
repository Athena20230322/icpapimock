(function () {

    // 新增檔案
    $('#fileSelect').on('click', function () {

        $('#formUpload').trigger('click');
    });

    var id = 'formUpload';
    var form = document.getElementById(id);
    var success = 0;
    var fail = 0;

    $("#lnkSave").click(function (e) {
        e.preventDefault();        

        $('#btnSubmit').trigger('click');
    });

    var evtRemovedfile = function (file) {
        this.removeFile(file);
    };
    var dropzone = new Dropzone("#" + id, {
        url: form.action,        
        autoProcessQueue: false,//停用自動上傳
        previewTemplate: $.trim(document.querySelector('#template-container').innerHTML), //模版要Trim掉空白
        addedfile: function (file) {//新增檔案

            //加入模版
            file.previewElement = Dropzone.createElement(this.options.previewTemplate);
            $('#formUpload ul').append(file.previewElement);
            var $li = $(file.previewElement);
            $li.find('.ufl-holder:first').text(file.name);

            //刪除事件
            var _this = this;
            $li.find('.ufl-close').click(function () {
                evtRemovedfile.call(_this, file);
            });
        },
        processing: function (file) {
            this.options.autoProcessQueue = true;
        },
        error: function (file, response) { 
            //if (response.RtnCode != 1) {
            alert(file.name + "超過檔案限制大小，請重新上傳小於5MB之檔案。");
                //libs.alert.popup(response.RtnMsg);    
            //}
            //else
            //{                
            //    this.removeFile(file);                
            //}
        },
        success: function (file, response) {
            if (response.RtnCode == 1) {                
                this.removeFile(file);   
                success++;
            }
            else
            {
                alert(response.RtnMsg);
                fail++;
            }
            
        },
        queuecomplete: function () {//所有檔案上傳完成
            this.options.autoProcessQueue = false;
            alert("成功:" + success + "筆,失敗:" + fail + "筆");
        }
    });

    //上傳
    $(form).submit(function () {
        dropzone.processQueue();//
        return false;
    });



})();


