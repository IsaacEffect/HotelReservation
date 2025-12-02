import React from "react";
import "../styles/facturacion.css";

export default function Loader() {
    return (
        <div className="loader-container">
            <div className="loader"></div>
            <p>Cargando...</p>
        </div>
    );
}
