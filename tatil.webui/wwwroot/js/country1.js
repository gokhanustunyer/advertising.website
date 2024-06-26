﻿function getCities(func, active_city = "", active_disc = "", active_nei = "") {
    var cities = document.querySelector("#cities");
    var isHave = false;
    $.ajax({
        type: "POST",
        url: `/user/${func}`,
        success: (response) => {
            for (var i = 0; i < response.length; i++) {
                var option = document.createElement("option");
                option.id = response[i]['item1']; option.text = response[i]['item2'];
                cities.appendChild(option);
                if (active_city == response[i]['item2']) {
                    document.querySelector("#cities").options[i].setAttribute("selected", "selected");
                    isHave = true;
                };
            }
            if (isHave) { changeCity(document.querySelector("#cities"), active_disc, active_nei) }
        }
    })
};

var $districts = $("#districts");
var changeCity = (obj, active_district = "", active_nei = "") => {
    var isHave = false;
    var districts = document.querySelector("#districts");
    $('#districts option').remove();
    $('#neighborhood option').remove();
    $.ajax({
        type: "POST",
        url: "/user/GetDistrict",
        data: { id: obj.options[obj.selectedIndex].id },
        success: (response) => {
            for (var i = 0; i < response.length; i++) {
                var option = document.createElement("option");
                response[i]['item2'] = response[i]['item2'].replace('I', 'ı');
                response[i]['item2'] = response[i]['item2'].charAt(0).toUpperCase() + response[i]['item2'].slice(1).toLowerCase();
                option.id = response[i]['item1']; option.text = response[i]['item2'];
                districts.appendChild(option);
                if (active_district == response[i]['item2']) {
                    option.setAttribute("selected", "selected")
                    isHave = true;
                };
            }
            if (isHave) { changeDistrict(document.querySelector("#districts"), active_nei); }
            document.querySelector("#districts").removeAttribute("disabled");
        }
    })
}

var changeDistrict = (obj, active_nei = "") => {
    $.ajax({
        type: "POST",
        url: "/user/GetNeighborhood",
        data: { id: obj.options[obj.selectedIndex].id },
        success: (response) => {
            $('#neighborhood option').remove();
            for (var i = 0; i < response.length; i++) {
                response[i]['item2'] = response[i]['item2'].replace('I', 'ı');
                response[i]['item2'] = response[i]['item2'].charAt(0).toUpperCase() + response[i]['item2'].slice(1).toLowerCase();
                $('#neighborhood').append(`<option id="${response[i]['item1']}">${response[i]['item2']}</options>`);
                if (active_nei == response[i]['item2']) {
                    document.querySelector("#neighborhood").options[i].setAttribute("selected", "selected");
                };
            }
            document.querySelector("#neighborhood").removeAttribute("disabled");
        }
    })
} 
