"use client"

import { useState } from "react"
import { reviewService } from "../services/reviewService"
import ReviewForm from "./ReviewForm"

function ReviewItem({ review, onReviewUpdated }) {
  const [isEditing, setIsEditing] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)
  const [error, setError] = useState("")

  const formatDate = (dateString) => {
    const date = new Date(dateString)
    return date.toLocaleDateString("en-US", {
      year: "numeric",
      month: "long",
      day: "numeric",
    })
  }

  const handleDelete = async () => {
    if (window.confirm("Are you sure you want to delete this review?")) {
      setIsDeleting(true)
      try {
        await reviewService.deleteReview(review.id)
        if (onReviewUpdated) {
          onReviewUpdated()
        }
      } catch (error) {
        setError(error.message)
      } finally {
        setIsDeleting(false)
      }
    }
  }

  const handleEditComplete = () => {
    setIsEditing(false)
    if (onReviewUpdated) {
      onReviewUpdated()
    }
  }

  if (isEditing) {
    return <ReviewForm bookId={review.bookId} existingReview={review} onReviewSubmitted={handleEditComplete} />
  }

  return (
    <div className="review-item">
      {error && <div className="alert alert-danger">{error}</div>}
      <div className="review-header">
        <div className="reviewer-info">
          <span className="reviewer-name">{review.userName}</span>
          <span className="review-date">{formatDate(review.createdAt)}</span>
          {review.updatedAt && review.updatedAt !== review.createdAt && <span className="review-edited">(edited)</span>}
        </div>
        <div className="review-rating">
          {"★".repeat(review.rating)}
          {"☆".repeat(5 - review.rating)}
        </div>
      </div>
      <div className="review-content">
        <p>{review.comment}</p>
      </div>
      {review.isOwner && (
        <div className="review-actions">
          <button className="btn btn-secondary btn-sm" onClick={() => setIsEditing(true)} disabled={isDeleting}>
            Edit
          </button>
          <button className="btn btn-danger btn-sm" onClick={handleDelete} disabled={isDeleting}>
            {isDeleting ? "Deleting..." : "Delete"}
          </button>
        </div>
      )}
    </div>
  )
}

export default ReviewItem
