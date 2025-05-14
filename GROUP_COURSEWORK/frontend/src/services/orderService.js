import { API_URL } from "../config"

const getOrders = async () => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/orders`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fetch orders")
  }

  return await response.json()
}

// Add this function to match what's being called in OrdersList
const getUserOrders = async () => {
  return getOrders() // This simply calls the existing getOrders function
}

const getOrderById = async (orderId) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/orders/${orderId}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fetch order details")
  }

  return await response.json()
}

const createOrder = async (orderData) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/orders`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(orderData),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to create order")
  }

  return await response.json()
}

const cancelOrder = async (orderId) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/orders/${orderId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to cancel order")
  }

  return await response.json()
}

export const orderService = {
  getOrders,
  getUserOrders, // Add this to the exported object
  getOrderById,
  createOrder,
  cancelOrder,
}
