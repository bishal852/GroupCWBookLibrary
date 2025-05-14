"use client"

import { useState, useEffect } from "react"
import { useParams, Link } from "react-router-dom"
import { API_URL } from "../../config"
import { wishlistService } from "../../services/wishlistService"
import { useCart } from "../../contexts/CartContext"
import ReviewsList from "../../components/ReviewsList"

function BookDetails() {
  const { id } = useParams()
  const [book, setBook] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")
  const [isInWishlist, setIsInWishlist] = useState(false)
  const [wishlistLoading, setWishlistLoading] = useState(false)
  const [wishlistMessage, setWishlistMessage] = useState("")

  const { addToCart } = useCart()
  const [cartQuantity, setCartQuantity] = useState(1)
  const [addingToCart, setAddingToCart] = useState(false)
  const [cartMessage, setCartMessage] = useState("")

  useEffect(() => {
    fetchBookDetails()
    checkWishlistStatus()
  }, [id])

  const fetchBookDetails = async () => {
    setLoading(true)
    try {
      const response = await fetch(`${API_URL}/api/books/${id}`)
      const data = await response.json()

      if (!response.ok) {
        throw new Error(data.message || "Failed to fetch book details")
      }

      setBook(data)
    } catch (error) {
      setError(error.message)
    } finally {
      setLoading(false)
    }
  }

  const checkWishlistStatus = async () => {
    try {
      const inWishlist = await wishlistService.isBookInWishlist(Number.parseInt(id))
      setIsInWishlist(inWishlist)
    } catch (error) {
      console.error("Error checking wishlist status:", error)
    }
  }

  const handleWishlistToggle = async () => {
    setWishlistLoading(true)
    setWishlistMessage("")
    try {
      if (isInWishlist) {
        await wishlistService.removeFromWishlist(Number.parseInt(id))
        setIsInWishlist(false)
        setWishlistMessage("Book removed from wishlist")
      } else {
        await wishlistService.addToWishlist(Number.parseInt(id))
        setIsInWishlist(true)
        setWishlistMessage("Book added to wishlist")
      }
    } catch (error) {
      setWishlistMessage(error.message)
    } finally {
      setWishlistLoading(false)
      // Clear message after 3 seconds
      setTimeout(() => {
        setWishlistMessage("")
      }, 3000)
    }
  }

  const handleAddToCart = async () => {
    setAddingToCart(true)
    setCartMessage("")
    try {
      await addToCart(Number.parseInt(id), cartQuantity)
      setCartMessage("Book added to cart")
      setCartQuantity(1)
    } catch (error) {
      setCartMessage(error.message)
    } finally {
      setAddingToCart(false)
      // Clear message after 3 seconds
      setTimeout(() => {
        setCartMessage("")
      }, 3000)
    }
  }

  const handleQuantityChange = (e) => {
    const value = Number.parseInt(e.target.value)
    if (value > 0 && value <= book.stockQuantity) {
      setCartQuantity(value)
    }
  }

  const isDiscountActive =
    book?.isOnSale &&
    book?.discountPrice &&
    book?.discountStartDate &&
    book?.discountEndDate &&
    new Date() >= new Date(book.discountStartDate) &&
    new Date() <= new Date(book.discountEndDate)

  const currentPrice = isDiscountActive ? book?.discountPrice : book?.price

  if (loading) {
    return <div className="loading">Loading book details...</div>
  }

  if (error) {
    return <div className="alert alert-danger">{error}</div>
  }

  if (!book) {
    return <div className="alert alert-danger">Book not found</div>
  }

  return (
    <div className="book-details">
      <div className="book-details-header">
        <Link to="/user-panel" className="back-link">
          &larr; Back to Catalogue
        </Link>
      </div>

      <div className="book-details-content">
        <div className="book-details-image">
          <img
            src={book.coverImageUrl || "/images/book-placeholder.jpg"}
            alt={`Cover of ${book.title}`}
            className="book-cover-large"
          />
          {isDiscountActive && <div className="sale-badge-large">On Sale</div>}
        </div>

        <div className="book-details-info">
          <h1 className="book-details-title">{book.title}</h1>
          <h2 className="book-details-author">by {book.author}</h2>

          <div className="book-details-rating">
            {"★".repeat(Math.floor(book.averageRating))}
            {"☆".repeat(5 - Math.floor(book.averageRating))}
            <span className="rating-value">({book.averageRating.toFixed(1)})</span>
          </div>

          <div className="book-details-price">
            {isDiscountActive && <span className="original-price">${book.price.toFixed(2)}</span>}
            <span className={isDiscountActive ? "discount-price" : ""}>${currentPrice.toFixed(2)}</span>
          </div>

          <div className="book-details-availability">
            <span className={book.stockQuantity > 0 ? "in-stock" : "out-of-stock"}>
              {book.stockQuantity > 0 ? "In Stock" : "Out of Stock"}
            </span>
            {book.stockQuantity > 0 && <span className="stock-quantity">({book.stockQuantity} available)</span>}
          </div>

          <div className="book-details-actions">
            <div className="add-to-cart-container">
              <div className="quantity-selector">
                <label htmlFor="quantity">Qty:</label>
                <input
                  type="number"
                  id="quantity"
                  min="1"
                  max={book.stockQuantity}
                  value={cartQuantity}
                  onChange={handleQuantityChange}
                  disabled={book.stockQuantity <= 0 || addingToCart}
                  className="quantity-input"
                />
              </div>
              <button
                className="btn btn-primary"
                onClick={handleAddToCart}
                disabled={book.stockQuantity <= 0 || addingToCart}
              >
                {addingToCart ? "Adding..." : "Add to Cart"}
              </button>
            </div>
            <button
              className={`btn ${isInWishlist ? "btn-danger" : "btn-secondary"}`}
              onClick={handleWishlistToggle}
              disabled={wishlistLoading}
            >
              {wishlistLoading ? "Processing..." : isInWishlist ? "Remove from Wishlist" : "Add to Wishlist"}
            </button>
          </div>

          {cartMessage && <div className="alert alert-success">{cartMessage}</div>}

          {wishlistMessage && (
            <div className={`alert ${isInWishlist ? "alert-success" : "alert-info"}`}>{wishlistMessage}</div>
          )}

          <div className="book-details-metadata">
            <div className="metadata-item">
              <span className="metadata-label">ISBN:</span>
              <span className="metadata-value">{book.isbn}</span>
            </div>
            <div className="metadata-item">
              <span className="metadata-label">Publisher:</span>
              <span className="metadata-value">{book.publisher}</span>
            </div>
            <div className="metadata-item">
              <span className="metadata-label">Publication Date:</span>
              <span className="metadata-value">{new Date(book.publicationDate).toLocaleDateString()}</span>
            </div>
            <div className="metadata-item">
              <span className="metadata-label">Language:</span>
              <span className="metadata-value">{book.language}</span>
            </div>
            <div className="metadata-item">
              <span className="metadata-label">Format:</span>
              <span className="metadata-value">{book.format}</span>
            </div>
            <div className="metadata-item">
              <span className="metadata-label">Genre:</span>
              <span className="metadata-value">{book.genre}</span>
            </div>
          </div>
        </div>
      </div>

      <div className="book-details-description">
        <h3 className="description-title">Description</h3>
        <p className="description-text">{book.description}</p>
      </div>

      <div className="book-details-reviews">
        <ReviewsList bookId={Number.parseInt(id)} />
      </div>
    </div>
  )
}

export default BookDetails
