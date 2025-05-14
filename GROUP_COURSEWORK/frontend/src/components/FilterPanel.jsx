"use client"

import { useState, useEffect } from "react"
import { API_URL } from "../config"

function FilterPanel({ filters, onFilterChange }) {
  const [genres, setGenres] = useState([])
  const [authors, setAuthors] = useState([])
  const [publishers, setPublishers] = useState([])
  const [languages, setLanguages] = useState([])
  const [formats, setFormats] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")
  const [isExpanded, setIsExpanded] = useState(false)
  const [priceRange, setPriceRange] = useState({
    min: filters.minPrice || "",
    max: filters.maxPrice || "",
  })

  useEffect(() => {
    fetchFilterOptions()
  }, [])

  const fetchFilterOptions = async () => {
    setLoading(true)
    try {
      const [genresRes, authorsRes, publishersRes, languagesRes, formatsRes] = await Promise.all([
        fetch(`${API_URL}/api/books/genres`),
        fetch(`${API_URL}/api/books/authors`),
        fetch(`${API_URL}/api/books/publishers`),
        fetch(`${API_URL}/api/books/languages`),
        fetch(`${API_URL}/api/books/formats`),
      ])

      const [genresData, authorsData, publishersData, languagesData, formatsData] = await Promise.all([
        genresRes.json(),
        authorsRes.json(),
        publishersRes.json(),
        languagesRes.json(),
        formatsRes.json(),
      ])

      setGenres(genresData)
      setAuthors(authorsData)
      setPublishers(publishersData)
      setLanguages(languagesData)
      setFormats(formatsData)
    } catch (error) {
      setError("Failed to load filter options")
      console.error(error)
    } finally {
      setLoading(false)
    }
  }

  const handleGenreChange = (genre) => {
    onFilterChange({ ...filters, genre: filters.genre === genre ? null : genre })
  }

  const handleAuthorChange = (author) => {
    onFilterChange({ ...filters, author: filters.author === author ? null : author })
  }

  const handlePublisherChange = (publisher) => {
    onFilterChange({ ...filters, publisher: filters.publisher === publisher ? null : publisher })
  }

  const handleLanguageChange = (language) => {
    onFilterChange({ ...filters, language: filters.language === language ? null : language })
  }

  const handleFormatChange = (format) => {
    onFilterChange({ ...filters, format: filters.format === format ? null : format })
  }

  const handlePriceChange = (e) => {
    const { name, value } = e.target
    setPriceRange({ ...priceRange, [name]: value })
  }

  const applyPriceFilter = () => {
    onFilterChange({
      ...filters,
      minPrice: priceRange.min === "" ? null : Number.parseFloat(priceRange.min),
      maxPrice: priceRange.max === "" ? null : Number.parseFloat(priceRange.max),
    })
  }

  const handleCheckboxChange = (e) => {
    const { name, checked } = e.target
    onFilterChange({ ...filters, [name]: checked })
  }

  const clearAllFilters = () => {
    setPriceRange({ min: "", max: "" })
    onFilterChange({
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
  }

  if (loading) {
    return <div className="filter-panel-loading">Loading filters...</div>
  }

  if (error) {
    return <div className="filter-panel-error">{error}</div>
  }

  return (
    <div className="filter-panel">
      <div className="filter-header">
        <h3>Filters</h3>
        <button className="btn btn-sm btn-secondary" onClick={() => setIsExpanded(!isExpanded)}>
          {isExpanded ? "Collapse" : "Expand"}
        </button>
      </div>

      <div className={`filter-content ${isExpanded ? "expanded" : ""}`}>
        <div className="filter-section">
          <h4>Price Range</h4>
          <div className="price-range">
            <input
              type="number"
              name="min"
              placeholder="Min"
              value={priceRange.min}
              onChange={handlePriceChange}
              className="form-control price-input"
              min="0"
            />
            <span>to</span>
            <input
              type="number"
              name="max"
              placeholder="Max"
              value={priceRange.max}
              onChange={handlePriceChange}
              className="form-control price-input"
              min="0"
            />
            <button className="btn btn-sm btn-primary" onClick={applyPriceFilter}>
              Apply
            </button>
          </div>
        </div>

        <div className="filter-section">
          <h4>Availability</h4>
          <div className="filter-checkbox">
            <input
              type="checkbox"
              id="onSaleOnly"
              name="onSaleOnly"
              checked={filters.onSaleOnly || false}
              onChange={handleCheckboxChange}
            />
            <label htmlFor="onSaleOnly">On Sale Only</label>
          </div>
          <div className="filter-checkbox">
            <input
              type="checkbox"
              id="inStockOnly"
              name="inStockOnly"
              checked={filters.inStockOnly || false}
              onChange={handleCheckboxChange}
            />
            <label htmlFor="inStockOnly">In Stock Only</label>
          </div>
        </div>

        {genres.length > 0 && (
          <div className="filter-section">
            <h4>Genre</h4>
            <div className="filter-options">
              {genres.map((genre) => (
                <div key={genre} className="filter-option">
                  <input
                    type="radio"
                    id={`genre-${genre}`}
                    name="genre"
                    checked={filters.genre === genre}
                    onChange={() => handleGenreChange(genre)}
                  />
                  <label htmlFor={`genre-${genre}`}>{genre}</label>
                </div>
              ))}
            </div>
          </div>
        )}

        {authors.length > 0 && isExpanded && (
          <div className="filter-section">
            <h4>Author</h4>
            <div className="filter-options">
              {authors.map((author) => (
                <div key={author} className="filter-option">
                  <input
                    type="radio"
                    id={`author-${author}`}
                    name="author"
                    checked={filters.author === author}
                    onChange={() => handleAuthorChange(author)}
                  />
                  <label htmlFor={`author-${author}`}>{author}</label>
                </div>
              ))}
            </div>
          </div>
        )}

        {formats.length > 0 && isExpanded && (
          <div className="filter-section">
            <h4>Format</h4>
            <div className="filter-options">
              {formats.map((format) => (
                <div key={format} className="filter-option">
                  <input
                    type="radio"
                    id={`format-${format}`}
                    name="format"
                    checked={filters.format === format}
                    onChange={() => handleFormatChange(format)}
                  />
                  <label htmlFor={`format-${format}`}>{format}</label>
                </div>
              ))}
            </div>
          </div>
        )}

        {languages.length > 0 && isExpanded && (
          <div className="filter-section">
            <h4>Language</h4>
            <div className="filter-options">
              {languages.map((language) => (
                <div key={language} className="filter-option">
                  <input
                    type="radio"
                    id={`language-${language}`}
                    name="language"
                    checked={filters.language === language}
                    onChange={() => handleLanguageChange(language)}
                  />
                  <label htmlFor={`language-${language}`}>{language}</label>
                </div>
              ))}
            </div>
          </div>
        )}

        {publishers.length > 0 && isExpanded && (
          <div className="filter-section">
            <h4>Publisher</h4>
            <div className="filter-options">
              {publishers.map((publisher) => (
                <div key={publisher} className="filter-option">
                  <input
                    type="radio"
                    id={`publisher-${publisher}`}
                    name="publisher"
                    checked={filters.publisher === publisher}
                    onChange={() => handlePublisherChange(publisher)}
                  />
                  <label htmlFor={`publisher-${publisher}`}>{publisher}</label>
                </div>
              ))}
            </div>
          </div>
        )}

        <button className="btn btn-secondary btn-sm clear-filters" onClick={clearAllFilters}>
          Clear All Filters
        </button>
      </div>
    </div>
  )
}

export default FilterPanel
