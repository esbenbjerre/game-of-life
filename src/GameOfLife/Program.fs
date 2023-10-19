open System
open GameOfLife

[<EntryPoint>]
let main args =
  (Console.WindowWidth, Console.WindowHeight)
  ||> Grid.random
  |> IO.run
  0
