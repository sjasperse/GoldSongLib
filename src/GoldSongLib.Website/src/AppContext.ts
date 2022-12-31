import React from "react"
import { User } from "./types"

type AppContextValues = {
  user: User | null
}

export const AppContext = React.createContext<AppContextValues>({
  user: null
})