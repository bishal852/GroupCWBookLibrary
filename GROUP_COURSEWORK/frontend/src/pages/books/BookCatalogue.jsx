"use client"

import { useState, useEffect } from "react"
import { API_URL } from "../../config"
import BookCard from "../../components/BookCard"
import SearchBar from "../../components/SearchBar"
import SortDropdown from "../../components/SortDropdown"
import FilterPanel from "../../components/FilterPanel"

function BookCatalogue() {
  const [books, setBooks] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(0)
  const [pageSize] = useState(10)
  const [searchTerm, setSearchTerm] = useState("")
  const [sortBy, setSortBy] = useState("CreatedAt")
  const [sortDescending, setSortDescending] = useState(true)
  const [filters, setFilters] = useState({
    genre: null,
    author: null,
    publisher: null,
    language: null,
    format: null,
    minPrice: null,
    maxPrice: null,
    onSaleOnly: false,
    inStockOnly: false,
  })
  const [showFilters, setShowFilters] = useState(false)

  useEffect(() => {
    fetchBooks()
  }, [currentPage, pageSize, searchTerm, sortBy, sortDescending, filters])

  const fetchBooks = async () => {
    setLoading(true)
    try {
      // Build query parameters
      const params = new URLSearchParams({
        page: currentPage.toString(),
        size: pageSize.toString(),
        sortBy,
        sortDescending: sortDescending.toString(),
      })

      if (searchTerm) {
        params.append("search", searchTerm)
      }

      if (filters.genre) {
        params.append("genre", filters.genre)
      }

      if (filters.author) {
        params.append("author", filters.author)
      }

      if (filters.publisher) {
        params.append("publisher", filters.publisher)
      }

      if (filters.language) {
        params.append("language", filters.language)
      }

      if (filters.format) {
        params.append("format", filters.format)
      }

      if (filters.minPrice) {
        params.append("minPrice", filters.minPrice.toString())
      }

      if (filters.maxPrice) {
        params.append("maxPrice", filters.maxPrice.toString())
      }

      if (filters.onSaleOnly) {
        params.append("onSaleOnly", "true")
      }

      if (filters.inStockOnly) {
        params.append("inStockOnly", "true")
      }

      const response = await fetch(`${API_URL}/api/books?${params.toString()}`)
      const data = await response.json()

      if (!response.ok) {
        throw new Error(data.message || "Failed to fetch books")
      }

      // Filter out any books that might still be marked as deleted
      const filteredBooks = data.items.filter((book) => !book.title.startsWith("[DELETED]"))

      setBooks(filteredBooks)
      setTotalPages(data.totalPages)
      setCurrentPage(data.currentPage)
    } catch (error) {
      setError(error.message)
    } finally {
      setLoading(false)
    }
  }

  const handleSearch = (term) => {
    setSearchTerm(term)
    setCurrentPage(1) // Reset to first page on new search
  }

  const handleSortChange = (newSortBy, newSortDescending) => {
    setSortBy(newSortBy)
    setSortDescending(newSortDescending)
    setCurrentPage(1) // Reset to first page on sort change
  }

  const handleFilterChange = (newFilters) => {
    setFilters(newFilters)
    setCurrentPage(1) // Reset to first page on filter change
  }

  const handlePreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1)
    }
  }

  const handleNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1)
    }
  }

  const toggleFilters = () => {
    setShowFilters(!showFilters)
  }

  return (
    <div className="book-catalogue">
      <h2 className="catalogue-title">Book Catalogue</h2>

      <div className="catalogue-controls">
        <SearchBar onSearch={handleSearch} />
        <div className="controls-right">
          <SortDropdown sortBy={sortBy} sortDescending={sortDescending} onSortChange={handleSortChange} />
          <button className="btn btn-secondary filter-toggle" onClick={toggleFilters}>
            {showFilters ? "Hide Filters" : "Show Filters"}
          </button>
        </div>
      </div>

      {error && <div className="alert alert-danger">{error}</div>}

      <div className="catalogue-layout">
        {showFilters && <FilterPanel filters={filters} onFilterChange={handleFilterChange} />}

        <div className="catalogue-main">
          {loading ? (
            <div className="loading">Loading books...</div>
          ) : (
            <>
              <div className="books-grid">
                {books.length > 0 ? (
                  books.map((book) => <BookCard key={book.id} book={book} />)
                ) : (
                  <p className="no-books">No books found matching your criteria.</p>
                )}
              </div>

              {totalPages > 0 && (
                <div className="pagination">
                  <button className="btn btn-secondary" onClick={handlePreviousPage} disabled={currentPage === 1}>
                    Previous
                  </button>
                  <span className="page-info">
                    Page {currentPage} of {totalPages}
                  </span>
                  <button className="btn btn-secondary" onClick={handleNextPage} disabled={currentPage === totalPages}>
                    Next
                  </button>
                </div>
              )}
            </>
          )}
        </div>
      </div>
    </div>
  )
}

export default BookCatalogue