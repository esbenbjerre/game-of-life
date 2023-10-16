module GameOfLife

open System

let neighbours (x, y) =
  [| (x + 1, y)
     (x, y + 1)
     (x - 1, y)
     (x, y - 1)
     (x + 1, y - 1)
     (x - 1, y + 1)
     (x - 1, y - 1)
     (x + 1, y + 1) |]
  |> Array.distinct

let isAlive grid (x, y) = grid |> Array.contains (x, y)

let aliveNeighbours grid (x, y) = neighbours (x, y) |> Array.filter (isAlive grid) |> Array.length

let survivors grid =
  grid
  |> Array.filter (fun (x, y) ->
    let n = (x,y) |> aliveNeighbours grid
    n = 2 || n = 3)

let births grid =
  grid
  |> Array.collect neighbours
  |> Array.distinct
  |> Array.filter (fun (x, y) -> (x, y) |> aliveNeighbours grid = 3)

let tick grid =
  let next = survivors grid |> Array.append (births grid)
  next |> Array.distinct

let printAt (x, y) (c: char) = Console.Write $"\027[{y + 1};{x + 1}H{c}"

let random width height =
  let r = Random()

  [| 0..width |]
  |> Array.collect (fun x -> [| 0..height |] |> Array.map (fun y -> x, y))
  |> Array.filter (fun _ -> r.Next(0, 5) > 3)