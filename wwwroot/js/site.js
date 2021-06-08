// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).on('submit', '#Registrar', function (e){
    e.preventDefault();
    $.ajax({
        beforeSend: function(){
            $('#Registrar button[type-submit]').prop('disabled', true);
        },
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function (data){
            alert("Usuario registrado con exito");
        },
        error: function (xhr, status){
            alert(xhr.responseJSON.title);
        },
        complete: function (){
            $('#Registrar button[type-submit]').prop('disabled', false);
        }
    })
})

$(document).on('submit', '#Login', function (e){
    e.preventDefault();
    $.ajax({
        beforeSend: function(){
            $('#Login button[type-submit]').prop('disabled', true);
        },
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function (data){
            alert("Bienvenido " + data);
            console.log(data);
            window.location= "/Home";
        },
        error: function (xhr, status, error){
            alert(xhr.responseJSON.title);            
        },
        complete: function (){
            $('#Login button[type-submit]').prop('disabled', false);
        }
    })
})

$(document).on('submit', '#Tarea', function (e){
    e.preventDefault();
    $.ajax({
        beforeSend: function(){
            $('#Tarea button[type-submit]').prop('disabled', true);
        },
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function (data){
            alert(data);
            console.log(data);
            window.location= "/Home";
        },
        error: function (xhr, status, error){
            alert(xhr.responseJSON.title);  
            window.location= "/Home";          
        },
        complete: function (){
            $('#Tarea button[type-submit]').prop('disabled', false);
        }
    })
})

