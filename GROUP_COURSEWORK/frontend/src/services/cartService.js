import { API_URL } from "../config"

const getCart = async () => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/cart`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fetch cart")
  }

  return await response.json()
}

const addToCart = async (bookId, quantity = 1) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/cart`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ bookId, quantity }),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to add book to cart")
  }

  return await response.json()
}

const updateCartItem = async (cartItemId, quantity) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/cart/${cartItemId}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ quantity }),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to update cart item")
  }

  return await response.json()
}

const removeFromCart = async (cartItemId) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/cart/${cartItemId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to remove item from cart")
  }

  return await response.json()
}

const clearCart = async () => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/cart`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to clear cart")
  }

  return await response.json()
}

export const cartService = {
  getCart,
  addToCart,
  updateCartItem,
  removeFromCart,
  clearCart,
}
