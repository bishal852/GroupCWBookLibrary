import { API_URL } from "../config"

const getOrderByClaimCode = async (claimCode) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/staff/order/${claimCode}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fetch order")
  }

  return await response.json()
}

const fulfillOrder = async (claimCode) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/staff/fulfill`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ claimCode }),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fulfill order")
  }

  return await response.json()
}

export const staffService = {
  getOrderByClaimCode,
  fulfillOrder,
}
