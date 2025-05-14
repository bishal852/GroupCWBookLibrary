import { API_URL } from "../config"

export const bannerService = {
  getAllBanners: async () => {
    try {
      const token = localStorage.getItem("token")
      const response = await fetch(`${API_URL}/api/banner`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      if (!response.ok) {
        throw new Error("Failed to fetch banners")
      }

      return await response.json()
    } catch (error) {
      console.error("Error fetching banners:", error)
      throw error
    }
  },

  getBannerById: async (id) => {
    try {
      const token = localStorage.getItem("token")
      const response = await fetch(`${API_URL}/api/banner/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      if (!response.ok) {
        throw new Error("Failed to fetch banner")
      }

      return await response.json()
    } catch (error) {
      console.error(`Error fetching banner ${id}:`, error)
      throw error
    }
  },

  getActiveBanner: async () => {
    try {
      const response = await fetch(`${API_URL}/api/banner/active`)

      if (!response.ok) {
        if (response.status === 404) {
          return null // No active banner
        }
        throw new Error("Failed to fetch active banner")
      }

      return await response.json()
    } catch (error) {
      console.error("Error fetching active banner:", error)
      throw error
    }
  },

  createBanner: async (bannerData) => {
    try {
      const token = localStorage.getItem("token")

      // Ensure dates are in ISO format
      const formattedData = {
        ...bannerData,
        startDate: new Date(bannerData.startDate).toISOString(),
        endDate: new Date(bannerData.endDate).toISOString(),
      }

      console.log("Creating banner with data:", formattedData)

      const response = await fetch(`${API_URL}/api/banner`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(formattedData),
      })

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}))
        const errorMessage = errorData.message || `Failed to create banner: ${response.status} ${response.statusText}`
        console.error("Banner creation error:", errorMessage)
        throw new Error(errorMessage)
      }

      return await response.json()
    } catch (error) {
      console.error("Error creating banner:", error)
      throw error
    }
  },

  updateBanner: async (id, bannerData) => {
    try {
      const token = localStorage.getItem("token")

      // Ensure dates are in ISO format
      const formattedData = {
        ...bannerData,
        startDate: new Date(bannerData.startDate).toISOString(),
        endDate: new Date(bannerData.endDate).toISOString(),
      }

      const response = await fetch(`${API_URL}/api/banner/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(formattedData),
      })

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}))
        const errorMessage = errorData.message || "Failed to update banner"
        throw new Error(errorMessage)
      }

      return await response.json()
    } catch (error) {
      console.error(`Error updating banner ${id}:`, error)
      throw error
    }
  },

  deleteBanner: async (id) => {
    try {
      const token = localStorage.getItem("token")
      const response = await fetch(`${API_URL}/api/banner/${id}`, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })

      if (!response.ok) {
        throw new Error("Failed to delete banner")
      }

      return true
    } catch (error) {
      console.error(`Error deleting banner ${id}:`, error)
      throw error
    }
  },
}
