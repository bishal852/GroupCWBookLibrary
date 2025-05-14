"use client"

import { useState, useEffect } from "react"
import { Link } from "react-router-dom"
import { wishlistService } from "../../services/wishlistService"
import UserNavbar from "../../components/UserNavbar";
import "../../styles/wishlist.css";



function Wishlist() {
  const [wishlistItems, setWishlistItems] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")

  useEffect(() => {
    fetchWishlist()
  }, [])

  const fetchWishlist = async () => {
    setLoading(true)
    try {
      const data = await wishlistService.getWishlist()
      setWishlistItems(data)
    } catch (error) {
      setError(error.message)
    } finally {
      setLoading(false)
    }
  }

  const handleRemoveFromWishlist = async (bookId) => {
    try {
      await wishlistService.removeFromWishlist(bookId)
      // Update the wishlist after removing the book
      setWishlistItems(wishlistItems.filter((item) => item.bookId !== bookId))
    } catch (error) {
      setError(error.message)
    }
  }

  if (loading) {
    return (
      <>
        <UserNavbar /> {/* Add the UserNavbar here */}
        <div className="loading">Loading wishlist...</div>
      </>
    )
  }

  if (error) {
    return (
      <>
        <UserNavbar /> {/* Add the UserNavbar here */}
        <div className="alert alert-danger">{error}</div>
      </>
    )
  }

  return (
    <>
      <UserNavbar /> {/* Add the UserNavbar here */}
      <div className="wishlist-page">
        <h2 className="wishlist-title">My Wishlist</h2>

        {wishlistItems.length === 0 ? (
          <div className="empty-wishlist">
            <p>Your wishlist is empty.</p>
            <Link to="/user-panel" className="btn btn-primary">
              Browse Books
            </Link>
          </div>
        ) : (
          <div className="wishlist-items">
            {wishlistItems.map((item) => (
              <div key={item.id} className="wishlist-item">
                <div className="wishlist-item-image">
                  <img
                    src={item.coverImageUrl || "/images/book-placeholder.jpg"}
                    alt={`Cover of ${item.title}`}
                    className="wishlist-cover"
                  />
                </div>
                <div className="wishlist-item-details">
                  <h3 className="wishlist-item-title">
                    <Link to={`/books/${item.bookId}`}>{item.title}</Link>
                  </h3>
                  <p className="wishlist-item-author">by {item.author}</p>
                  <div className="wishlist-item-price">
                    {item.isDiscountActive && <span className="original-price">${item.price.toFixed(2)}</span>}
                    <span className={item.isDiscountActive ? "discount-price" : ""}>${item.currentPrice.toFixed(2)}</span>
                  </div>
                  <div className="wishlist-item-actions">
                    <Link to={`/books/${item.bookId}`} className="btn btn-primary btn-sm">
                      View Details
                    </Link>
                    <button className="btn btn-danger btn-sm" onClick={() => handleRemoveFromWishlist(item.bookId)}>
                      Remove
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </>
  )
}
export default Wishlist
