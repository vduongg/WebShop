function userType(type, id) {


    //var user = document.getElementById('user').value;
    //var pass = document.getElementById('pass').value;

    if (type !== "") {
        document.getElementById(id).style.paddingTop = '0';
        document.getElementById(id).style.fontSize = '12px';
    }
    else {
        
        document.getElementById(id).style.paddingTop = '10px';
        document.getElementById(id).style.fontSize = '14px';
    }
}
