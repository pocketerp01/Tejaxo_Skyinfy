
function mytoast (type,pos,msg ) {
    toastr.options = {
        "debug": false,
        "positionClass": pos,
        "onclick": null,
        "fadeIn": 300,
        "fadeOut": 100,
        "timeOut": 3000,
        "extendedTimeOut": 1000
    }
        
    var d= Date();
    toastr[type](msg,type);  
};

//mytoast('success','toast-top-right',"Data Saved")
//<div class="span2">
//    <div class="control-group" id="toastTypeGroup">
//        <div class="controls">
//            <label>Toast Type</label>
//            <label class="radio">
//                <input type="radio" name="toasts" value="success" checked />Success
//                        </label>
//            <label class="radio">
//                <input type="radio" name="toasts" value="info" />Info
//                        </label>
//            <label class="radio">
//                <input type="radio" name="toasts" value="warning" />Warning
//                        </label>
//            <label class="radio">
//                <input type="radio" name="toasts" value="error" />Error
//                        </label>
//        </div>
//    </div>
//    <div class="control-group" id="positionGroup">
//        <div class="controls">
//            <label>Position</label>
//            <label class="radio">
//                <input type="radio" name="positions" value="toast-top-right" checked />Top Right
//                        </label>
//            <label class="radio">
//                <input type="radio" name="positions" value="toast-bottom-right" />Bottom Right
//                        </label>
//            <label class="radio">
//                <input type="radio" name="positions" value="toast-bottom-left" />Bottom Left
//                        </label>
//            <label class="radio">
//                <input type="radio" name="positions" value="toast-top-left" />Top Left
//                        </label>
//            <label class="radio">
//                <input type="radio" name="positions" value="toast-top-full-width" />Top Full Width
//                        </label>
//            <label class="radio">
//                <input type="radio" name="positions" value="toast-bottom-full-width" />Bottom Full Width
//                        </label>
//            <label class="radio">
//                <input type="radio" name="positions" value="toast-top-center" />Top Center
//                        </label>
//            <label class="radio">
//                <input type="radio" name="positions" value="toast-bottom-center" />Bottom Center
//                        </label>
//        </div>
//    </div>
//</div>