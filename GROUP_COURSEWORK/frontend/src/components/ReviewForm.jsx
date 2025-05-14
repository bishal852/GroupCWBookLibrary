"use client"

import { useState } from "react"
import { reviewService } from "../services/reviewService"

function ReviewForm({ bookId, onReviewSubmitted, existingReview = null }) {
  const [formData, setFormData] = useState({
    bookId: bookId,
    rating: existingReview ? existingReview.rating : 5,
    comment: existingReview ? existingReview.comment : "",
  })

  const [errors, setErrors] = useState({})
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [serverError, setServerError] = useState("")
  const [success, setSuccess] = useState("")

  const handleChange = (e) => {
    const { name, value } = e.target
    setFormData({
      ...formData,
      [name]: name === "rating" ? Number.parseInt(value) : value,
    })
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: "",
      })
    }
  }

  const validateForm = () => {
    const newErrors = {}

    if (formData.rating < 1 || formData.rating > 5) {
      newErrors.rating = "Rating must be between 1 and 5"
    }

    if (!formData.comment.trim()) {
      newErrors.comment = "Comment is required"
    } else if (formData.comment.length > 1000) {
      newErrors.comment = "Comment cannot exceed 1000 characters"
    }

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSubmit = async (e) => {
    e.preventDefault()

    // Reset messages
    setServerError("")
    setSuccess("")

    // Validate form
    if (!validateForm()) {
      return
    }

    setIsSubmitting(true)

    try {
      if (existingReview) {
        // Update existing review
        await reviewService.updateReview(existingReview.id, formData)
        setSuccess("Review updated successfully!")
      } else {
        // Create new review
        await reviewService.createReview(formData)
        setSuccess("Review submitted successfully!")
        // Reset form after successful submission
        setFormData({
          ...formData,
          rating: 5,
          comment: "",
        })
      }

      // Notify parent component
      if (onReviewSubmitted) {
        onReviewSubmitted()
      }
    } catch (error) {
      setServerError(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div className="review-form-container">
      <h3 className="review-form-title">{existingReview ? "Edit Your Review" : "Write a Review"}</h3>

      {serverError && <div className="alert alert-danger">{serverError}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      <form onSubmit={handleSubmit} className="review-form">
        <div className="form-group">
          <label htmlFor="rating">Rating</label>
          <div className="rating-input">
            {[1, 2, 3, 4, 5].map((star) => (
              <label key={star} className="star-label">
                <input
                  type="radio"
                  name="rating"
                  value={star}
                  checked={formData.rating === star}
                  onChange={handleChange}
                  className="star-input"
                />
                <span className={`star ${formData.rating >= star ? "selected" : ""}`}>★</span>
              </label>
            ))}
          </div>
          {errors.rating && <div className="invalid-feedback">{errors.rating}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="comment">Review</label>
          <textarea
            id="comment"
            name="comment"
            className={`form-control ${errors.comment ? "is-invalid" : ""}`}
            value={formData.comment}
            onChange={handleChange}
            rows={5}
            placeholder="Share your thoughts about this book..."
          />
          {errors.comment && <div className="invalid-feedback">{errors.comment}</div>}
          <div className="character-count">
            <small>{formData.comment.length}/1000 characters</small>
          </div>
        </div>

        <div className="form-actions">
          <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
            {isSubmitting ? "Submitting..." : existingReview ? "Update Review" : "Submit Review"}
          </button>
        </div>
      </form>
    </div>
  )
}

export default ReviewForm
