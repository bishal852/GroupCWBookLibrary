"use client"

import { useState } from "react"
import { Link } from "react-router-dom"
import { useCart } from "../contexts/CartContext"

function BookCard({ book }) {
  const [isHovered, setIsHovered] = useState(false)
  const { addToCart } = useCart()
  const [addingToCart, setAddingToCart] = useState(false)

  const isDiscountActive =
    book.isOnSale &&
    book.discountPrice &&
    book.discountStartDate &&
    book.discountEndDate &&
    new Date() >= new Date(book.discountStartDate) &&
    new Date() <= new Date(book.discountEndDate)

  const currentPrice = isDiscountActive ? book.discountPrice : book.price

  const handleAddToCart = async (e) => {
    e.preventDefault()
    e.stopPropagation()
    setAddingToCart(true)
    try {
      await addToCart(book.id, 1)
    } catch (error) {
      console.error("Error adding to cart:", error)
    } finally {
      setAddingToCart(false)
    }
  }

  return (
    <div className="book-card" onMouseEnter={() => setIsHovered(true)} onMouseLeave={() => setIsHovered(false)}>
      <div className="book-cover">
        <img
          src={book.coverImageUrl || "/images/book-placeholder.jpg"}
          alt={`Cover of ${book.title}`}
          className="book-image"
        />
        {isDiscountActive && <div className="sale-badge">On Sale</div>}
      </div>
      <div className="book-info">
        <h3 className="book-title">{book.title}</h3>
        <p className="book-author">by {book.author}</p>
        <div className="book-rating">
          {"★".repeat(Math.floor(book.averageRating))}
          {"☆".repeat(5 - Math.floor(book.averageRating))}
          <span className="rating-value">({book.averageRating.toFixed(1)})</span>
        </div>
        <div className="book-price">
          {isDiscountActive && <span className="original-price">${book.price.toFixed(2)}</span>}
          <span className={isDiscountActive ? "discount-price" : ""}>${currentPrice.toFixed(2)}</span>
        </div>
        {isHovered && (
          <div className="book-actions">
            <Link to={`/books/${book.id}`} className="btn btn-primary btn-sm">
              View Details
            </Link>
            <button
              className="btn btn-secondary btn-sm"
              onClick={handleAddToCart}
              disabled={addingToCart || book.stockQuantity <= 0}
            >
              {addingToCart ? "Adding..." : "Add to Cart"}
            </button>
          </div>
        )}
      </div>
    </div>
  )
}

export default BookCard
