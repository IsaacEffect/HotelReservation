import React from "react";
import "../styles/facturacion.css";

export default function ErrorMessage({ mensaje }) {
    return (
        <div className="error-box">
            ?? {mensaje}
        </div>
    );
}
