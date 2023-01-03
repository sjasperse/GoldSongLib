import React from "react"
import { Song, User } from "./types"

export type AppContextValues = {
  user: User | null,
  songs: Song[] | null,
  refreshSongs: () => Promise<void>
}

export const AppContext = React.createContext<AppContextValues>({
  user: null,
  songs: [],
  refreshSongs: () => Promise.resolve()
})