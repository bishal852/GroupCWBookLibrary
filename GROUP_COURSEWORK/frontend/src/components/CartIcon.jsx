"use client"

import { Link } from "react-router-dom"
import { useCart } from "../contexts/CartContext"

function CartIcon() {
  const { cart, loading } = useCart()

  const itemCount = cart?.totalItems || 0

  return (
    <Link to="/cart" className="cart-icon-link">
      <div className="cart-icon">
        <i className="fa fa-shopping-cart" style={{ marginRight: '8px', fontSize: '35px', color: 'black' }}></i>

        {!loading && itemCount > 0 && <span className="cart-count">{itemCount}</span>}
      </div>
    </Link>
  )
}

export default CartIcon
