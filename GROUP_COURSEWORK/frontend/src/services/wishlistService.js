import { API_URL } from "../config"

const getWishlist = async () => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/wishlist`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fetch wishlist")
  }

  return await response.json()
}

const addToWishlist = async (bookId) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/wishlist`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ bookId }),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to add book to wishlist")
  }

  return await response.json()
}

const removeFromWishlist = async (bookId) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/wishlist/${bookId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to remove book from wishlist")
  }

  return await response.json()
}

const isBookInWishlist = async (bookId) => {
  try {
    const wishlist = await getWishlist()
    return wishlist.some((item) => item.bookId === bookId)
  } catch (error) {
    console.error("Error checking if book is in wishlist:", error)
    return false
  }
}

export const wishlistService = {
  getWishlist,
  addToWishlist,
  removeFromWishlist,
  isBookInWishlist,
}
