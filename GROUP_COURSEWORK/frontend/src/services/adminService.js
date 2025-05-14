import { API_URL } from "../config"

export const adminService = {
  createStaff: async (staffData) => {
    const token = localStorage.getItem("token")

    try {
      const response = await fetch(`${API_URL}/api/admin/createstaff`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(staffData),
      })

      const data = await response.json()

      if (!response.ok) {
        throw new Error(data.message || "Failed to create staff")
      }

      return data
    } catch (error) {
      console.error("Error creating staff:", error)
      throw error
    }
  },

  // Add other admin-related API calls here
}
