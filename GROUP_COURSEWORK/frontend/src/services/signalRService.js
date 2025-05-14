import * as signalR from "@microsoft/signalr"

class SignalRService {
  constructor() {
    this.connection = null
    this.baseUrl = "http://localhost:5000" // Update with your API URL
  }

  // Initialize the connection
  async start() {
    try {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(`${this.baseUrl}/hubs/orders`)
        .withAutomaticReconnect()
        .build()

      // Set up event handlers before starting the connection
      this.connection.on("ReceiveOrderNotification", (message) => {
        if (window.addToast) {
          window.addToast(message)
        }
      })

      await this.connection.start()
      console.log("SignalR Connected")
      return true
    } catch (err) {
      console.error("SignalR Connection Error: ", err)
      return false
    }
  }

  // Stop the connection
  async stop() {
    if (this.connection) {
      await this.connection.stop()
      console.log("SignalR Disconnected")
    }
  }
}

// Create a singleton instance
const signalRService = new SignalRService()
export default signalRService
