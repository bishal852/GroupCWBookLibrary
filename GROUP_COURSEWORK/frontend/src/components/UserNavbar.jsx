"use client"

import { Link } from "react-router-dom"
import CartIcon from "./CartIcon" // Adjust path if needed
import "font-awesome/css/font-awesome.min.css"

function UserNavbar({ onLogout }) {
    return (
        <div className="panel-header">
            <Link to="/user-panel" style={{ textDecoration: 'none', color: 'inherit' }}>
                <h2 className="panel-title">Book Library</h2>
            </Link>

            <div className="panel-actions">
                <Link to="/wishlist">
                    <i className="fa fa-heart" style={{ marginRight: '10px', fontSize: '31px', color: 'black' }}></i>
                </Link>

                <Link to="/orders">
                    <i className="fa fa-clipboard" style={{ marginRight: '8px', fontSize: '30px', color: 'black' }}></i>
                </Link>

                <CartIcon />

                {onLogout && (
                    <button onClick={onLogout} className="btn btn-danger">
                        Logout
                    </button>
                )}
            </div>
        </div>
    )
}

export default UserNavbar
