function show(StartTime) {
    //alert(StartTime);
    var Digital = new Date()
    var hours = Digital.getHours()
    var minutes = Digital.getMinutes()
    var seconds = Digital.getSeconds()    
    
    if (StartTime != null) {
        var arrsttime = StartTime.split(':');
        var sthh = parseInt(arrsttime[0]);
        var stmi = parseInt(arrsttime[1]);

        stmi = stmi + (sthh * 60);

        var mi = minutes + (parseInt(hours) * 60);
        var crntmin = mi - stmi;
        
        hours = Math.floor(crntmin/60) % 24;
        minutes = crntmin % 60;
    }

    var dn = "AM"
    if (hours > 12) {
        dn = "PM"
        hours = hours - 12
    }

    //if (hours == 0)
    //    hours = 12
    if (minutes <= 9)
        minutes = "0" + minutes
    if (seconds <= 9)
        seconds = "0" + seconds

    var lblTimer = document.getElementById('lblTimer');
    if (lblTimer != null) {
        lblTimer.innerHTML = hours + ":" + minutes + ":" + seconds;
    }

    setTimeout("show(StartTime)", 1000);
}