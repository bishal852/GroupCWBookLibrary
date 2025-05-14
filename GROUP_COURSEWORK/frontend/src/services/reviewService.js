import { API_URL } from "../config"

const getBookReviews = async (bookId) => {
  const response = await fetch(`${API_URL}/api/reviews/book/${bookId}`)

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fetch reviews")
  }

  return await response.json()
}

const getReview = async (reviewId) => {
  const response = await fetch(`${API_URL}/api/reviews/${reviewId}`)

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to fetch review")
  }

  return await response.json()
}

const createReview = async (reviewData) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/reviews`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(reviewData),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to create review")
  }

  return await response.json()
}

const updateReview = async (reviewId, reviewData) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/reviews/${reviewId}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(reviewData),
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to update review")
  }

  return await response.json()
}

const deleteReview = async (reviewId) => {
  const token = localStorage.getItem("token")
  if (!token) {
    throw new Error("Authentication required")
  }

  const response = await fetch(`${API_URL}/api/reviews/${reviewId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message || "Failed to delete review")
  }

  return await response.json()
}

const canReviewBook = async (bookId) => {
  const token = localStorage.getItem("token")
  if (!token) {
    return { canReview: false, hasPurchased: false, hasReviewed: false }
  }

  try {
    const response = await fetch(`${API_URL}/api/reviews/can-review/${bookId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })

    if (!response.ok) {
      return { canReview: false, hasPurchased: false, hasReviewed: false }
    }

    return await response.json()
  } catch (error) {
    console.error("Error checking if user can review book:", error)
    return { canReview: false, hasPurchased: false, hasReviewed: false }
  }
}

export const reviewService = {
  getBookReviews,
  getReview,
  createReview,
  updateReview,
  deleteReview,
  canReviewBook,
}
