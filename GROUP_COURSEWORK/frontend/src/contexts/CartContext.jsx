"use client"

import { createContext, useState, useContext, useEffect } from "react"
import { cartService } from "../services/cartService"

const CartContext = createContext()

export const useCart = () => useContext(CartContext)

export const CartProvider = ({ children }) => {
  const [cart, setCart] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  const fetchCart = async () => {
    setLoading(true)
    try {
      const cartData = await cartService.getCart()
      setCart(cartData)
      setError(null)
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    // Only fetch cart if user is authenticated
    const token = localStorage.getItem("token")
    if (token) {
      fetchCart()
    } else {
      setLoading(false)
    }
  }, [])

  const addToCart = async (bookId, quantity = 1) => {
    try {
      await cartService.addToCart(bookId, quantity)
      await fetchCart() // Refresh cart data
      return true
    } catch (err) {
      setError(err.message)
      return false
    }
  }

  const updateCartItem = async (cartItemId, quantity) => {
    try {
      await cartService.updateCartItem(cartItemId, quantity)
      await fetchCart() // Refresh cart data
      return true
    } catch (err) {
      setError(err.message)
      return false
    }
  }

  const removeFromCart = async (cartItemId) => {
    try {
      await cartService.removeFromCart(cartItemId)
      await fetchCart() // Refresh cart data
      return true
    } catch (err) {
      setError(err.message)
      return false
    }
  }

  const clearCart = async () => {
    try {
      await cartService.clearCart()
      await fetchCart() // Refresh cart data
      return true
    } catch (err) {
      setError(err.message)
      return false
    }
  }

  return (
    <CartContext.Provider
      value={{
        cart,
        loading,
        error,
        addToCart,
        updateCartItem,
        removeFromCart,
        clearCart,
        refreshCart: fetchCart,
      }}
    >
      {children}
    </CartContext.Provider>
  )
}
