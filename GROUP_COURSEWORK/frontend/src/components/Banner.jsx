"use client"

import { useState, useEffect } from "react"
import { bannerService } from "../services/bannerService"

const Banner = () => {
  const [banner, setBanner] = useState(null)
  const [loading, setLoading] = useState(true)
  const [dismissed, setDismissed] = useState(false)

  useEffect(() => {
    const fetchActiveBanner = async () => {
      try {
        const activeBanner = await bannerService.getActiveBanner()
        setBanner(activeBanner)
      } catch (error) {
        console.error("Error fetching active banner:", error)
      } finally {
        setLoading(false)
      }
    }

    fetchActiveBanner()
  }, [])

  const handleDismiss = () => {
    setDismissed(true)
  }

  if (loading || !banner || dismissed) {
    return null
  }

  return (
    <div
      className="banner"
      style={{
        backgroundColor: banner.backgroundColor,
        color: banner.textColor,
      }}
    >
      <div className="banner-content">
        <p>{banner.message}</p>
      </div>

    </div>
  )
}

export default Banner
