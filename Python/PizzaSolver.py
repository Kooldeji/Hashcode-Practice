import math

constraint = None
pizza = []
bestCount = 0
bestScore = math.inf


class Constraint:
    def __init__(self, r, c, l, h):
        self.__l = l
        self.__h = h
        self.__r = r
        self.__c = c

    def __getattr__(self, val):
        if val == "l":
            return self.__l
        elif val == "h":
            return self.__h
        elif val == "r":
            return self.__r
        elif val == "c":
            return self.__c

class Slice:
    def __init__(self, r1, c1, r2, c2):
        self.__r1 = r1
        self.__r2 = r2
        self.__c1 = c1
        self.__c2 = c2
        self.__area = (abs(self.__r2 - self.__r1) + 1) * (abs(self.__c2 - self.__c1) + 1)

    @property
    def r1(self):
        return self.__r1

    @property
    def r2(self):
        return self.__r2

    @property
    def c1(self):
        return self.__c1

    @property
    def c2(self):
        return self.__c2

    @property
    def area(self):
        return self.__area

    def cut(self):
        for i in range(self.__r1, self.__r2 + 1):
            for j in range(self.__c1, self.__c2 + 1):
                cell = pizza[i][j]
                cell.isCut = True

    def unCut(self):
        for i in range(self.__r1, self.__r2 + 1):
            for j in range(self.__c1, self.__c2 + 1):
                cell = pizza[i][j]
                cell.isCut = False

    def isValid(self):
        shrooms = 0
        tomatoes = 0
        for i in range(self.__r1, self.__r2 + 1):
            for j in range(self.__c1, self.__c2 + 1):
                cell = pizza[i][j]
                if (cell.isCut):
                    return False
                if (cell.isShroom):
                    shrooms += 1
                else:
                    tomatoes += 1
        if self.__area > constraint.h or self.__area == 1:
            return False
        if shrooms < constraint.l or tomatoes < constraint.l:
            return False
        return True


class Cell:
    def __init__(self, r, c, i):
        self.__r = r
        self.__c = c
        self.__isShroom = not i
        self.isCut = False

    @property
    def isShroom(self):
        return self.__isShroom

    def getSlices(self):
        i = 0
        slices = []
        while i < constraint.h and self.__c + i < constraint.c:
            j = 0
            while (i + 1) * (j + 1) <= constraint.h and self.__r + j < constraint.r:
                newSlice = Slice(self.__r, self.__c, self.__r + j, self.__c + i)
                if newSlice.isValid():
                    slices.append(newSlice)
                    # TODO score and rank slices
                j += 1
            i += 1
        return slices


def count_cell(pizza):
    count = 0
    for row_cells in pizza:
        for cell in row_cells:
            if not cell.isCut:
                count += 1
    return count


def count_cut_cell(pizza):
    count = 0
    for row_cells in pizza:
        for cell in row_cells:
            if cell.isCut:
                count += 1
    return count


def getHashCode(query):
    hashed_value = ""
    for i in range(len(query)):
        for j in range(len(query[i])):
            if query[i][j].isCut:
                hashed_value = "".join((hashed_value, "0"))
            else:
                hashed_value = "".join((hashed_value, "1"))

    return hashed_value


def ps(pizza, store, bestSlices):
    pizza_hashcode = getHashCode(pizza)
    if pizza_hashcode in store:
        return store[pizza_hashcode]

    score = math.inf
    best_slice = None

    has_slice = False
    for row_cells in pizza:
        for cell in row_cells:
            if cell.isCut:
                continue
            for SLICE in cell.getSlices():
                SLICE.cut()
                current_best_score = ps(pizza, store, bestSlices)
                if score > current_best_score:
                    score = current_best_score
                    best_slice = SLICE
                SLICE.unCut()
                has_slice = True
    if not has_slice:
        current_best_score = count_cell(pizza)
        if score > current_best_score:
            score = current_best_score

            global bestScore, bestCount
            if (bestScore > score):
                bestScore = score
                bestCount += 1
                print("{}. {}".format(bestCount, count_cut_cell(pizza)))
    store[pizza_hashcode] = score
    bestSlices[pizza_hashcode] = best_slice
    return store[pizza_hashcode]


def pizzaSlice(pizza):
    store = dict()
    bestSlices = dict()
    ps(pizza, store, bestSlices)
    pizza_hashcode = getHashCode(pizza)
    listOfSlices = []

    while pizza_hashcode in bestSlices and bestSlices[pizza_hashcode] is not None:
        SLICE = bestSlices[pizza_hashcode]
        listOfSlices.append(SLICE)
        SLICE.cut()
        pizza_hashcode = getHashCode(pizza)

    return listOfSlices


def main():
    inp("a.in")
    slices = pizzaSlice(pizza)
    out(slices)


def inp(filePath):
    global constraint, pizza
    file = open(filePath)
    r, c, l, h = map(int, file.readline().split(" "))
    constraint = Constraint(r, c, l, h)
    readlines = file.readlines()
    for row in range(0, len(readlines)):
        line = readlines[row]
        rows = line[:-1]
        cells = []
        for col in range(0, len(rows)):
            ing = rows[col]
            cells.append(Cell(row, col, ing == "T"))
        pizza.append(cells)


def out(slices):
    file = open("out.txt", "w")
    print(len(slices), file=file)
    for s in slices:
        print(s.r1, s.c1, s.r2, s.c2, file=file)


main()
