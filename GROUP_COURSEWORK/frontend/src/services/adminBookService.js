import { API_URL } from "../config"

const getBooks = async (queryParams = {}) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  // Build query string from params
  const params = new URLSearchParams()
  Object.entries(queryParams).forEach(([key, value]) => {
    if (value !== undefined && value !== null && value !== "") {
      params.append(key, value)
    }
  })

  const response = await fetch(`${API_URL}/api/admin/books?${params.toString()}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fetch books")
  }

  return await response.json()
}

const getBookById = async (id) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/admin/books/${id}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fetch book")
  }

  return await response.json()
}

const createBook = async (bookData) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/admin/books`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(bookData),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to create book")
  }

  return await response.json()
}

const updateBook = async (id, bookData) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/admin/books/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(bookData),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to update book")
  }

  return await response.json()
}

const deleteBook = async (id, force = false) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  // Build the URL with the force parameter if needed
  const url = force ? `${API_URL}/api/admin/books/${id}?force=true` : `${API_URL}/api/admin/books/${id}`

  console.log(`Deleting book ${id}, force=${force}, URL=${url}`)

  try {
    const response = await fetch(url, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })

    if (!response.ok) {
      const errorData = await response.json()
      console.error("Delete book error response:", errorData)
      throw new Error(errorData.message || `Failed to delete book with ID ${id}`)
    }

    const result = await response.json()
    console.log("Delete book success:", result)
    return result
  } catch (error) {
    // If the error is not from the API response (e.g., network error)
    if (!error.message.includes("Failed to delete") && !error.message.includes("not found")) {
      console.error("Network or other error:", error)
      throw new Error(`Network error while deleting book with ID ${id}. Please try again.`)
    }
    throw error
  }
}

const updateBookStock = async (id, stockQuantity) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/admin/books/${id}/stock`, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ stockQuantity }),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to update book stock")
  }

  return await response.json()
}

const setDiscount = async (discountData) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/admin/books/discounts`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(discountData),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to set discount")
  }

  return await response.json()
}

export const adminBookService = {
  getBooks,
  getBookById,
  createBook,
  updateBook,
  deleteBook,
  updateBookStock,
  setDiscount,
}
