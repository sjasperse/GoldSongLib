export type Song = {
  id: string,
  name: string,
  tags: string[]
}

export type WorshipOrder = {
  id: string,
  date: Date,
  songSets: WorshipOrderSongSet[],
  tags: string[]
}

export type WorshipOrderSongSet = {
  id: string,
  title: string,
  songs: WorshipOrderSong[]
}

export type WorshipOrderSong = {
  id: string,
  songId: string | null
}
