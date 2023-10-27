const _modeloProducto = {
    idProd: 0,
    nombre: "",
    descripcion: "",
    precio: 0,
    stock: 0,
    idCat: 0,
    fechaReg: ""
}

function MostrarProductos() {
    fetch("/Home/ListaProductos")
        .then(res => {
            return res.ok ? res.json() : Promise.reject(res)
        })
        .then(resJson => {
            if (resJson.length > 0) {
                $("#tableProducts tbody").html("");

                resJson.forEach((prod) => {
                    $("#tableProducts tbody").append(
                        $("<tr>").append(
                            $("<td>").text(prod.nombre),
                            $("<td>").text(prod.descripcion),
                            $("<td>").text(prod.precio),
                            $("<td>").text(prod.stock),
                            $("<td>").text(prod.refCategoria.nombre),
                            $("<td>").text(prod.fechaReg),
                            $("<td>").append(
                                $(document.createElement('button')).addClass("btn btn-primary btn-sm btn-editar").text("Editar").data("dataProducto", prod),
                                $(document.createElement('button')).addClass("btn btn-danger ms-2 btn-sm btn-eliminar").text("Eliminar").data("dataProducto", prod)
                            )
                        )
                    )
                })
            }
        })


}

document.addEventListener("DOMContentLoaded", function () {
    MostrarProductos()
    fetch("/Home/ListaCategorias")
        .then(res => {
            return res.ok ? res.json() : Promise.reject(res)
        })
        .then(resJson => {
            if (resJson.length > 0) {
                resJson.forEach((item) => {
                    $("#cboCategoria").append(
                        $("<option>").val(item.idCat).text(item.nombre)
                    )
                })
            }
        })

    // calendario
    $("#txtFechaReg").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    })
}, false)


// modal
function MostrarModal() {
    $("#txtNombre").val(_modeloProducto.nombre);
    $("#txtDescripcion").val(_modeloProducto.descripcion);
    $("#txtPrecio").val(_modeloProducto.precio);
    $("#txtStock").val(_modeloProducto.stock);
    $("#cboCategoria").val(_modeloProducto.idCat == 0 ? $("#cboCategoria option:first").val() : _modeloProducto.idCat);
    $("#txtFechaReg").val(_modeloProducto.fechaReg);

    $("#modalProducto").modal("show")
}








/**** */
$(document).on("click", ".btn-new-product", function () {
    _modeloProducto.idProd = 0;
    _modeloProducto.nombre = "";
    _modeloProducto.descripcion = "";
    _modeloProducto.precio = 0;
    _modeloProducto.stock = 0;
    _modeloProducto.idCat = 0;
    _modeloProducto.fechaReg = "";

    MostrarModal();

})

$(document).on("click", ".btn-editar", function () {

    const _producto = $(this).data("dataProducto");


    _modeloProducto.idProd = _producto.idProd;
    _modeloProducto.nombre = _producto.nombre;
    _modeloProducto.descripcion = _producto.descripcion;
    _modeloProducto.precio = _producto.precio;
    _modeloProducto.stock = _producto.stock;
    _modeloProducto.idCat = _producto.refCategoria.idCat;
    _modeloProducto.fechaReg = _producto.fechaReg

    MostrarModal();

})



// guardar
$(document).on("click", ".btn-guardar", function () {
    const modelo = {
        idProd: _modeloProducto.idProd,
        nombre: $("#txtNombre").val(),
        descripcion: $("#txtDescripcion").val(),
        precio: $("#txtPrecio").val(),
        stock: $("#txtStock").val(),
        refCategoria: {
            idCat: $("#cboCategoria").val()
        },
        fechaReg: $("#txtFechaReg").val(),

    }

    if (_modeloProducto.idProd == 0) {
        fetch("/Home/GuardarProducto", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(res => {
                return res.ok ? res.json() : Promise.reject(res)
            })
            .then(resJson => {
                if (resJson.valor) {
                    $("#modalProducto").modal("hide");
                    Swal.fire("Listo!", "Producto fue creado", "success");
                    MostrarProductos();
                }
                else {
                    Swal.fire("Lo sentimos!", "NO se puedo crear ", "error");
                }
            })

    } else {
        fetch("/Home/EditarProducto", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(res => {
                return res.ok ? res.json() : Promise.reject(res)
            })
            .then(resJson => {
                if (resJson.valor) {
                    $("#modalProducto").modal("hide");
                    Swal.fire("Listo!", "Producto fue actualizado", "success");
                    MostrarProductos();
                }
                else {
                    Swal.fire("Lo sentimos!", "NO se puedo actualizar ", "error");
                }
            })

    }
})




// eliminar
$(document).on("click", ".btn-eliminar", function () {
    const _producto = $(this).data("dataProducto");

    Swal.fire({
        title: "Estas seguro?",
        text: `Eliminar Producto "${_producto.nombre}"`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, volver"

    }).then((res) => {
        if (res.isConfirmed) {
            fetch(`/Home/EliminarProducto?id=${_producto.idProd}`, {
                method: "DELETE"
            })
                .then(res => {
                    return res.ok ? res.json() : Promise.reject(res)
                })
                .then(resJson => {
                    if (resJson.valor) {

                        Swal.fire("Listo!", "Producto fue eliminado ", "success");
                        MostrarProductos();
                    }

                    else {
                        Swal.fire("Lo sentimos!", "NO se puedo eliminar ", "error");
                    }
                })
                .catch(error => {
                    console.error('Error al realizar la solicitud:', error);
                });
        }
    })
})

