"use client"

import { useState, useEffect } from "react"
import { reviewService } from "../services/reviewService"
import ReviewItem from "./ReviewItem"
import ReviewForm from "./ReviewForm"

function ReviewsList({ bookId }) {
  const [reviews, setReviews] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")
  const [canReview, setCanReview] = useState(false)
  const [hasPurchased, setHasPurchased] = useState(false)
  const [hasReviewed, setHasReviewed] = useState(false)
  const [showReviewForm, setShowReviewForm] = useState(false)

  useEffect(() => {
    fetchReviews()
    checkCanReview()
  }, [bookId])

  const fetchReviews = async () => {
    setLoading(true)
    try {
      const data = await reviewService.getBookReviews(bookId)
      setReviews(data)
    } catch (error) {
      setError(error.message)
    } finally {
      setLoading(false)
    }
  }

  const checkCanReview = async () => {
    try {
      const {
        canReview: canReviewBook,
        hasPurchased: hasUserPurchased,
        hasReviewed: hasUserReviewed,
      } = await reviewService.canReviewBook(bookId)
      setCanReview(canReviewBook)
      setHasPurchased(hasUserPurchased)
      setHasReviewed(hasUserReviewed)
    } catch (error) {
      console.error("Error checking if user can review:", error)
    }
  }

  const handleReviewSubmitted = () => {
    fetchReviews()
    checkCanReview()
    setShowReviewForm(false)
  }

  if (loading) {
    return <div className="loading">Loading reviews...</div>
  }

  return (
    <div className="reviews-section">
      <h2 className="reviews-title">Customer Reviews</h2>

      {error && <div className="alert alert-danger">{error}</div>}

      <div className="reviews-summary">
        <div className="reviews-count">
          {reviews.length} {reviews.length === 1 ? "review" : "reviews"}
        </div>
        {canReview && (
          <button className="btn btn-primary" onClick={() => setShowReviewForm(true)}>
            Write a Review
          </button>
        )}
        {!canReview && hasPurchased && hasReviewed && (
          <div className="review-status">You have already reviewed this book</div>
        )}
        {!canReview && !hasPurchased && <div className="review-status">Only verified buyers can review this book</div>}
      </div>

      {showReviewForm && canReview && <ReviewForm bookId={bookId} onReviewSubmitted={handleReviewSubmitted} />}

      <div className="reviews-list">
        {reviews.length === 0 ? (
          <div className="no-reviews">No reviews yet. Be the first to review this book!</div>
        ) : (
          reviews.map((review) => (
            <ReviewItem key={review.id} review={review} onReviewUpdated={handleReviewSubmitted} />
          ))
        )}
      </div>
    </div>
  )
}

export default ReviewsList
